# line = "		public UInt32 flags { get; set; }"

import os
import os.path

def parseProperty(line):
    tokens = line.split()
    propertyType = tokens[1]
    structName = tokens[2]
    srcPropertyName = structName.capitalize()
    namedParameter = structName + " = pCreateInfo." + srcPropertyName + ","
    return namedParameter

f = open(os.path.curdir + '/Properties.txt')
for line in f.readlines():
    print(parseProperty(line))


