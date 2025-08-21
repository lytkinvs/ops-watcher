from pydantic import BaseModel


class Item(BaseModel):
    id: int = None
    date: str
    