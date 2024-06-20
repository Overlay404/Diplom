from fastapi import Depends, status, HTTPException, Cookie
from sqlalchemy.orm import Session
import models, database, oauth

def verificate_request(token, jwt_token, db: Session = Depends(database.get_db)):
    user = oauth.verificate_jwt(jwt_token, db)

    obj = db.query(models.Database).filter(
        models.Database.token == token
    ).first()

    if not obj:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND, detail=f"Database not found")
    
    if not user:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")
    
    if user.id != obj.user_id:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail=f"Invalid Credentials")
    
    return obj