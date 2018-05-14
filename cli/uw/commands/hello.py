from .base import Base
from json import dumps

class Hello(Base):
    def run(self):
        print "Hello from uw cli"
        print "You supplied the following options:%s" % dumps(self.options, indent=2, sort_keys=True)
