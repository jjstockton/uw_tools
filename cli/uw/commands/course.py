from .base import Base
from ..sql import SQLite

class Course(Base):
    def run(self):

        subject = self.options['<subject>'].upper()
        code = self.options['<code>']

        with SQLite() as conn:
            c = conn.cursor()

            c.execute("SELECT subject, code, term FROM courses WHERE subject = ? AND code = ?", (subject, code))

            rows = c.fetchall()

            for row in rows:
                print "%s" % row[2]

            print "\nReturned %s results." % len(rows)
