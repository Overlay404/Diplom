from passlib.context import CryptContext
import secrets
from fastapi import Depends
from sqlalchemy.orm import Session
import models, database

pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")


def hash(password: str):
    return pwd_context.hash(password)


def verify(plain_password, hashed_password):
    return pwd_context.verify(plain_password, hashed_password)


def generate_token(db: Session = Depends(database.get_db)):
    token = secrets.token_hex(16)

    database = db.query(models.Database).filter(
        models.Database.token == token
    ).first()

    if bool(database):
        generate_token(db)

    return token

def int_try_parse(value):
    try:
        int(value)
        return True
    except ValueError:
        return False