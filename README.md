# file_btree_indexing
demonstration of BTREE Indexing in files

Descendencia de "Field"
capacities
 -object to byte array (Pack)
 -byte array to object (UnPack)

Descendencia de "Record"
capacities
 -Record to byte array (Pack)
 -byte array to Record (UnPack)
add attrib sparator, attrib status and attrib end record

special "Records"
  - HeaderRecord
  - DeletedRecord

HeaderParser
- Manage "HeaderRecord" (Pack and UnPack)








                                                               XXXXX
			                    XXXXX			                  |                             XXXXX
		        XXXXX                           XXXXX             |             XXXXX                           XXXXX
	    XXXXX           XXXXX     |     XXXXX           XXXXX     |     XXXXX           XXXXX     |     XXXXX           XXXXX
	XXXXX   XXXXX | XXXXX   XXXXX | XXXXX   XXXXX | XXXXX   XXXXX | XXXXX   XXXXX | XXXXX   XXXXX | XXXXX   XXXXX | XXXXX   XXXXX
					

 
					  
					  
                                                                10000
			                    07000                             |                             20000
		        05000                           09000             |             15000                           30000
	    04000           06000     |     08000           09500     |     14000           16000     |     25000           40000
	03500   04500 | 05500   06100 | 07800   08100 | 09400   09600 | 13000   14500 | 15500   16500 | 24000   26000 | 39000   50000
					

A10000Z
A07000Z
A20000Z
A05000Z
A09000Z
A15000Z
A30000Z
A04000Z
A06000Z
A08000Z  
A09500Z
A14000Z 
A16000Z  
A25000Z  
A40000Z
A03500Z
A04500Z
A05500Z
A06100Z 
A07800Z
A08100Z
A09400Z
A09600Z 
A13000Z 
A14500Z
A15500Z
A16500Z 
A24000Z 
A26000Z 
A39000Z 
A50000Z
