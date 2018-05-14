"""
uw-cli

Usage:
    uw hello
    uw courses [--term=<term>] [--search=<search>] [--limit=<limit>]
    uw course <subject> <code>
    uw terms
    uw profs [--name=<name>] [--limit=<limit>]
    uw -h | --help
    uw --version

Options:
    -h --help               Show this screen.
    --version               Show version.
    --term=<term>           Specify the term for which the command is to be processed under.
    --limit=<limit>         Limit on the number of rows to return [default: 20].
    --search=<search>       Search the results.
""" 

from inspect import getmembers, isclass
from docopt import docopt
from . import __version__ as VERSION

def main():
    import uw.commands

    options = docopt(__doc__, version=VERSION)
 
    for (k, v) in options.items():
        if v and hasattr(uw.commands, k):
            module = getattr(uw.commands, k)
            uw.commands = getmembers(module, isclass)
            command = [command[1] for command in uw.commands if command[0] != 'Base' and command[0] != 'SQLite'][0]
            command = command(options)
            command.run()
