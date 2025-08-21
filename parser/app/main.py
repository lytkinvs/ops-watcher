
from fastapi import FastAPI
from .api import parse_list

app = FastAPI()

@app.get("/")
def get_root():
    return {}

app.include_router(parse_list.router)