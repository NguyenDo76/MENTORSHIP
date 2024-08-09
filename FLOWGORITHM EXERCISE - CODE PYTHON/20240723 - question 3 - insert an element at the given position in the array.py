import random as rd
def InsertElement (n,position,InsertValue,a):
  if position < 0 or position >= n:
    print ('Position out of range')
  else:
    Newa = [0] * (n+1)
    Newa[position] = InsertValue
    for i in range (0,position-1 + 1,1):
      Newa[i] = a[i]
    for i in range (position,n-1 + 1,1):
      Newa[i+1] = a[i]
    print ('Array after insertion: ')
    PrintArray (n+1,Newa)


def PrintArray (n,a):
  for i in range (0,n-1 + 1,1):
    print (a[i])

print ('Given an integer n, create an array with n random integers from 0 to 9. Insert an element at the given position in the array.')
print ('Enter number n = ')
n = int (input ())
a = []  # Initialize a as an empty list
for i in range (0,n-1 + 1,1):
  a.append(rd.randint (0,9))
print ('Original array: ')
PrintArray (n,a)
print ('Enter the position of the element to be inserted: ')
position = int (input ())
print ('Enter the value of the element to be inserted: ')
InsertValue = int (input ())
InsertElement (n,position,InsertValue,a)
