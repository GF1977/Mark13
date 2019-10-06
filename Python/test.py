import math

primary1 = 43
primary2 = 53
message = primary1 * primary2
print (message)

i=50041401
i_max = 50041501

while i<i_max:
    p = 3
    stop = 0
    while p<=math.sqrt(i) and not stop:
        ost = i%p
        if ost == 0:
            print (i," Not primary, divider = ",p)
            stop = 1
        p=p+2
    if not stop:
        print (i, "is Prime")
    i=i+2




