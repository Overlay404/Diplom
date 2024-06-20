from typing import List
from sqlalchemy import ForeignKey
from sqlalchemy import String
from sqlalchemy.orm import DeclarativeBase
from sqlalchemy.orm import Mapped
from sqlalchemy.orm import mapped_column
from sqlalchemy.orm import relationship
from pydantic import BaseModel

class Base(DeclarativeBase):
    pass

class Database(Base):
    __tablename__ = "databases"
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(String(30))
    token: Mapped[str] = mapped_column(String(100))
    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"))
    user = relationship("User" ,back_populates="databases")

class User(Base):
    __tablename__ = "users"
    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(30))
    login: Mapped[str] = mapped_column(String(30))
    password: Mapped[str] = mapped_column(String(30))
    token: Mapped[str] = mapped_column(String(100))
    databases = relationship("Database", back_populates="user")

class References(BaseModel):
    table: str
    field: str

class Field(BaseModel):
    name: str
    type_field: str
    is_primary_key: bool | None = None
    is_foregraund_key: bool | None = None
    references: References | None = None
    autoincrement: bool | None = None

class UserRequestLogin(BaseModel):
    login: str
    password: str

class UserRequestRegister(BaseModel):
    username: str
    login: str
    password: str