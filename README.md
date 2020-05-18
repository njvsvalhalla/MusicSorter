
# Music Sorter

  I am really picky of how I sort my music collection and I just made this in a hour or few to help me sort it since it's gotten super disoragnized. It's .NET core app so cross platform and blah blah blah

Eventually this will be customizable to sort music in a given folder but right now I have it set to output something like

> /Orchid/[FLAC] 2005 - Totality
> /City of Caterpillar/[MP3-VBR] 2002 - City of Caterpillar
> /Pageninetynine/[MP3-320] 2000 - Document 5

etc

## How to build?

It's just a normal .NET core app, so just hit it with a dotnet build

## How To Use

musicsorter **inputFolder**  **outputFolder**

Note, this does not actually move files yet, just copies. Moving files will happen in a more final version but for now I got plenty of space.

## Todo

 - I'd like to see splits/comps handled better but I'm ok with what I've seen it do for now
 - Move log + cue files along with everything
 - Check for errors better..
 - I'd like to also do something where it might convert flac to opus while moving stuff to make my life easier when I transfer files to my phone
 - Realistically I'd prefer if folder names had v2, v0 in them but trying to figure a good way to do that
