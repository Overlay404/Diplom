from fastapi import APIRouter, Depends, status, HTTPException, Cookie
from sqlalchemy.orm import Session
import models, database, utils, oauth
from typing import Annotated

router = APIRouter(tags=['Authentication'])


@router.post('/api/login')
def login(user: models.UserRequestLogin, db: Session = Depends(database.get_db)):

    userQuery = db.query(models.User).filter(
        models.User.login == user.login
    ).first()

    if not userQuery:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")

    if not utils.verify(user.password, userQuery.password):
      raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")

    return userQuery.token


@router.post('/api/register')
def register(user: models.UserRequestRegister, db: Session = Depends(database.get_db)):
    new_user = models.User(
        username = user.username,
        login = user.login,
        password = utils.hash(user.password),
        token=oauth.create_access_token(data = {"user_login": user.login})
    )

    db.add(new_user)
    db.commit()
    db.refresh(new_user)

    return new_user.token


@router.get('/api/check_login')
def check_login(login: str, db: Session = Depends(database.get_db)):
    userQuery = db.query(models.User).filter(
        models.User.login == login
    ).first()
    if not userQuery:
        return False
    else:
        return True


@router.get('/api/get_user')
def get_user(db: Session = Depends(database.get_db), jwt_token: Annotated[str | None, Cookie()] = None):
    user = oauth.verificate_jwt(jwt_token, db)
    if not user:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")
    
    return user.username


@router.get('/api/change_username')
def get_user(username: str, db: Session = Depends(database.get_db), jwt_token: Annotated[str | None, Cookie()] = None):
    user = oauth.verificate_jwt(jwt_token, db)
    if not user:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")
    
    user.username = username
    db.commit()
    return user.username