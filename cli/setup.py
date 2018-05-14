from setuptools import setup

setup(
    name='uw-cli',
    version='1.0',
    description='A useful module',
    author='Jacob Stockton',
    author_email='jacob.stockton@hotmail.com',
    packages=['uw'],  #same as name
    install_requires=['docopt', 'psycopg2'], #external packages as dependencies
    entry_points = {
        'console_scripts': [
            'uw=uw.cli:main',
        ],
    },
)
