import time
from playwright.sync_api import sync_playwright

from ..models.item import Item


def parse_list():
    with sync_playwright() as pw:
        url = ''

        browser = pw.chromium.connect('ws://playwright-server:3000/')

        context = browser.new_context()
        page = context.new_page()

        print("Navigating to the page...")
        page.goto(url)

        # Wait for initial content to load
        # page.wait_for_load_state('networkidle')

        print("Starting infinite scroll...")

        # Variables to track scroll progress
        last_height = 0
        scroll_count = 0
        max_scrolls = 10  # Limit to prevent infinite loops

        while scroll_count < max_scrolls:
            # Get current page height
            current_height = page.evaluate('document.body.scrollHeight')

            # Scroll to bottom
            page.evaluate('window.scrollTo(0, document.body.scrollHeight)')

            # Wait for new content to load
            time.sleep(2)

            # Wait for network to be idle (new content loaded)
            try:
                page.wait_for_load_state('networkidle', timeout=5000)
            except:
                print("Timeout waiting for new content, continuing...")

            # break
            # Check if new content was loaded
            new_height = page.evaluate('document.body.scrollHeight')

            if new_height == current_height:
                print(f"No new content loaded after scroll {scroll_count + 1}")
                # Try scrolling a bit more in case content is loading slowly
                time.sleep(3)
                new_height = page.evaluate('document.body.scrollHeight')
                if new_height == current_height:
                    print("Reached end of content or no more items to load")
                    break

            scroll_count += 1
            print(f"Scroll {scroll_count}: Height changed from {current_height} to {new_height}")


        print(f"Completed {scroll_count} scrolls")

        divs = page.locator("//div[starts-with(@class, 'styled__Card')]")
        count = divs.count()
        res = []
        months = {
            'января': '01', 'февраля': '02', 'марта': '03', 'апреля': '04',
            'мая': '05', 'июня': '06', 'июля': '07', 'августа': '08',
            'сентября': '09', 'октября': '10', 'ноября': '11', 'декабря': '12'
        }

        for i in range(count):
            text = divs.nth(i).locator('a').nth(0).get_attribute('href')
            _, _, vacancy_id = text.split('/')

            date_string = divs.nth(i).locator("//div[starts-with(@color, 'textSecondary')]")

            day, month, year = date_string.text_content().split(' ')
            date = f'{day}.{months[month]}.{year}'

            res.append(Item(date=date, id=int(vacancy_id)))

            res.sort(key=lambda x: x.id, reverse=True)

        print(res)
        browser.close()

    return res