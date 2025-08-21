
from fastapi import APIRouter, HTTPException
from typing import List
from ..models.item import Item
from ..parsers.parse_list import parse_list

router = APIRouter()


@router.get("/parse/list", response_model=List[Item])
def get_list():
    lst = parse_list()
    return lst