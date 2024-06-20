from sqlalchemy import create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker
from sqlalchemy.orm import Session
from models import Base
import sqlite3
from sqlite3 import Error
from pathlib import Path
import os

engine = create_engine("sqlite:///database.db", echo=True)

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

Base.metadata.create_all(bind=engine)

def get_db():
    db = Session(autoflush=False, bind=engine)
    try:
        yield db
    finally:
        db.close()



def create_connection(token):
    conn = sqlite3.connect(f"databases/{token}.db")
    return conn

def delete_db(token: str):
    if not os.path.isfile(f"databases/{token}.db"):
       return
    os.remove(f"databases/{token}.db")

def check_db(token: str):
    if not os.path.isfile(f"databases/{token}.db"):
        return False
    else:
        return True