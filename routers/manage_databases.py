from fastapi import APIRouter, Depends, status, HTTPException, Cookie
from sqlalchemy.orm import Session
from typing import Annotated
import models, database, utils, oauth, verification
import sqlite3

router = APIRouter(tags=['Manage_database'])


@router.get('/api/create_database')
def create_database(name_db: str, db: Session = Depends(database.get_db), jwt_token: Annotated[str | None, Cookie()] = None):

    token = utils.generate_token(db)
    user = oauth.verificate_jwt(jwt_token, db)

    token = token.replace("\"", "")

    if not user:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")
    

    new_database = models.Database(
        name = name_db,
        token = token,
        user_id = user.id
    )

    db.add(new_database)
    db.commit()
    db.refresh(new_database)
    conn = database.create_connection(token)
    cursor = conn.cursor()
    cursor.close()
    conn.close()

    return token.replace("\"", "")


@router.get('/api/get_databases')
async def get_databases(db: Session = Depends(database.get_db), jwt_token: Annotated[str | None, Cookie()] = None):
    user = oauth.verificate_jwt(jwt_token, db)
    if not user:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")
    databases = db.query(models.Database).all()
    str_json = list()
    for item in databases:
        if user.id != item.user_id:
            continue
        str_json.append({"name": item.name, "token": item.token.replace("\"", "")})

    return str_json

@router.get('/api/delete_database')
def delete_database(token: str, db: Session = Depends(database.get_db), jwt_token: Annotated[str | None, Cookie()] = None):
    obj = verification.verificate_request(token, jwt_token, db)
    database.delete_db(obj.token)
    db.delete(obj)
    db.commit()
    return token

@router.get('/api/rename_database')
def rename_database(token: str, name: str, db: Session = Depends(database.get_db), jwt_token: Annotated[str | None, Cookie()] = None):
    obj = verification.verificate_request(token, jwt_token, db)
    obj.name = name   
    db.commit()
    return token


@router.get('/api/check_db')
def check_db(token: str):
    f = database.check_db(token)
    return f

@router.get('/api/sql_query')
def query(query: str, token: str,  db: Session = Depends(database.get_db), jwt_token: Annotated[str | None, Cookie()] = None):
    user = oauth.verificate_jwt(jwt_token, db)
    if not user:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")

    isUserDatabase = False

    for item in user.databases:
        if(item.token == token):
            isUserDatabase = True

    if isUserDatabase is False:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")
    
    try:
        conn = database.create_connection(token)
        cursor = conn.cursor()
        cursor.execute(query)
        data = cursor.fetchall()
        conn.commit()
        cursor.close()
        conn.close()
        return data
    except Exception as e:
        return e.args
    