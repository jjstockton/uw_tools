from .base import Base
import sqlite3
import datetime

def get_current_term():
    today = datetime.date.today()
    return "1%s%s" % (today.year % 1000, today.month - ((today.month - 1) % 4))


# Currently courses that are not offered in a particular term are not included in the database for that term.
# If that is the intended behaviour then that's fine. However, maybe we should have a way of returning all courses
# in the calendar for a particular term/year (regardless of whether they are offered or not).

# For implementing searching, maybe mimic grep 
class Courses(Base):
    def run(self):

        if self.options['--term']:
            term = self.options['--term']
        else:
            term = get_current_term()

        limit = self.options['--limit']

        if self.options['--search']:
            search = "%" + self.options['--search'] + "%"
        else:
            search = "%%"


        conn = sqlite3.connect('test.db')
        c = conn.cursor()

        c.execute('SELECT subject, code, title FROM courses WHERE term = ? AND (title LIKE ? OR subject LIKE ?) ORDER BY subject, code LIMIT ?', (term, search, search, limit))

        rows = c.fetchall()

        for row in rows:        	
            print "%s %s - %s" % (row[0], row[1], row[2])

        print "\nReturned %s results." % len(rows)

        conn.close()
