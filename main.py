from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from routers import auth, manage_databases,database_router



app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(auth.router)
app.include_router(manage_databases.router)
app.include_router(database_router.router)

@app.get("/")
def read_root():
    return {"Hello": "World"}

