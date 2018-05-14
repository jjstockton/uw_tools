from .base import Base
from ..sql import SQLite

class Terms(Base):
    def run(self):
        
        with SQLite() as conn:
            c = conn.cursor()

            c.execute("SELECT term FROM log")

            rows = c.fetchall()

            for row in rows:
                print row[0]


