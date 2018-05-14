from .base import Base
from ..sql import SQLite

class Profs(Base):
    def run(self):

        limit = self.options['--limit']

        if self.options['--name']:
            search = "%" + self.options['--name'] + "%"
        else:
            search = "%%"

        with SQLite() as conn:
            c = conn.cursor()

            c.execute('SELECT DISTINCT instructor FROM class_times WHERE instructor IS NOT NULL AND instructor LIKE ? LIMIT ?', (search, limit))

            rows = c.fetchall()

            for row in rows:        	
                print "%s" % (row[0])

            print "\nReturned %s results." % len(rows)
