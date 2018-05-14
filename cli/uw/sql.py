import sqlite3

class SQLite(object):

    def __enter__(self):
        self.conn = sqlite3.connect('test.db')

        return self.conn

    def __exit__(self, exc_type, exc_value, traceback):
        self.conn.close()