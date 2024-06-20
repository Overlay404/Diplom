from fastapi import APIRouter, Depends, status, HTTPException, Cookie
from sqlalchemy.orm import Session
import models, database, verification, utils
import sqlite3

router = APIRouter(tags=['Database_router'])

@router.post('/api/create_table')
def create_table(table_name: str, token: str, fields: list[models.Field]):

    query = f'CREATE TABLE {table_name} ('

    for field in fields:
        if field.autoincrement:
          query += f'{field.name} {field.type_field} PRIMARY KEY AUTOINCREMENT,'
          continue
        if field.is_primary_key:
          query += f'{field.name} {field.type_field} PRIMARY KEY,'
          continue
        if field.is_foregraund_key:
            query += f'{field.name} {field.type_field}, FOREIGN KEY ({field.name})  REFERENCES {field.references.table} ({field.references.field}),'
            continue
        query += f'{field.name} {field.type_field},'

    query = query[:-1]
    query += ');'
    try:
        connect_execute_query(token, query)
    except Exception as ex:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST, detail=f"The server could not process your request, please check the entered data. Your exception: {ex}")
    return {"status": "success",
            "database": token }

@router.get('/api/delete_table')
def delete_table(table_name: str, token: str):
    query = f'DROP TABLE {table_name};'
    try:
        connect_execute_query(token, query)
    except Exception as ex:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST, detail=f"The server could not process your request, please check the entered data. Your exception: {ex}")
    return {"status": "success",
            "database": token }

@router.post('/api/{table_name}/add')
def add_data(table_name: str, token: str, values: dict):

    query = f'INSERT INTO {table_name} ('

    for value in values:
        if value == "id" and values[value] == 0:
            continue
        query += f'{value},'

    query = query[:-1]

    query += ') VALUES ('
    
    for value in values:
        if value == "id" and values[value] == 0:
            continue
        if utils.int_try_parse(values[value]):
            query += f'{values[value]},'
        else:
            query += f"'{values[value]}'," 
    query = query[:-1]
    query += ');'

    try:
        connect_execute_query(token, query)
    except Exception as ex:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST, detail=f"The server could not process your request, please check the entered data. Your exception: {ex}")
    return {"status": "success",
            "database": token }

@router.post('/api/{table_name}/update')
def update_data(table_name: str, token: str, old_value: dict, new_value: dict):

    query = f'UPDATE {table_name} SET '
    for value in new_value:
        query += f'{value} = '
        if utils.int_try_parse(new_value[value]):
            query += f'{new_value[value]},'
        else:
            query += f"'{new_value[value]}'," 
    query = query[:-1]
    query += f' WHERE '
    for value in old_value:
        query += f'{value} = '
        if utils.int_try_parse(old_value[value]):
            query += f'{old_value[value]} AND '
        else:
            query += f"'{old_value[value]}' AND " 
    query = query[:-5]
    query += ';'

    try:
        connect_execute_query(token, query)
    except Exception as ex:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST, detail=f"The server could not process your request, please check the entered data. Your exception: {ex}")
    return token

@router.get('/api/{table_name}/delete')
def delete_data(id: str,table_name: str, token: str):
   
    query = f'DELETE FROM {table_name} WHERE id = {id}'

    try:
        connect_execute_query(token, query)
    except Exception as ex:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST, detail=f"The server could not process your request, please check the entered data. Your exception: {ex}")
    return {"status": "success",
            "database": token }

@router.get('/api/get_tables')
def get_tables(token: str):
    return get_table(f"SELECT name FROM sqlite_master WHERE type = 'table'", token)

@router.get('/api/{table_name}/get')
def get_data(table_name: str,  token: str, id: str | None = None):
   
    if id != None:
        row = get_table(f"SELECT * FROM {table_name} WHERE id = {id}", token)
        return row

    row = get_table(f"SELECT * FROM {table_name}", token)

    return row

@router.get('/api/{table_name}/get_table_info')
def get_data(table_name: str,  token: str):
    row = get_table(f"PRAGMA table_info('{table_name}');", token)
    return row

def get_table(query: str, token):
    try:
        conn = database.create_connection(token)
        conn.row_factory = sqlite3.Row
        cursor = conn.cursor()
        cursor.execute(query)
        row = [dict((cursor.description[i][0], value) \
            for i, value in enumerate(row)) for row in cursor.fetchall()]
        cursor.close()
        conn.close()
    except Exception as ex:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST, detail=f"The server could not process your request, please check the entered data. Your exception: {ex}")
    
    return row

def connect_execute_query(token, query):
    conn = database.create_connection(token)
    cursor = conn.cursor()
    cursor.execute(query)
    conn.commit()
    cursor.close()
    conn.close()