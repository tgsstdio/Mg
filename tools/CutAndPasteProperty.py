# line = "		public UInt32 flags { get; set; }"

import os
import os.path

def parseProperty(line):
    tokens = line.split()
    propertyType = tokens[1]
    structName = tokens[2]
    srcPropertyName = structName[0].upper() + structName[1:]
    if structName == 'sType':
        namedParameter = "sType = VkStructureType.StructureType,"
    elif structName == 'pNext':
        namedParameter = "pNext = IntPtr.Zero,"
    elif propertyType.endswith('Flags'):
        namedParameter = structName + " = (" + propertyType + ") pCreateInfo." + srcPropertyName + ","
    elif propertyType == 'VkBool32':
        namedParameter = structName + " = VkBool32.ConvertTo(pCreateInfo." + srcPropertyName + "),"
    else:
        namedParameter = structName + " = pCreateInfo." + srcPropertyName + ","
    
    return namedParameter

f = open(os.path.curdir + '/Properties.txt')
for line in f.readlines():
    print(parseProperty(line))


