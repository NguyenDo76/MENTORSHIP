import random as rd
def DeleteElement (n,position,a):
  if position < 0 or position >= n:
    print ('Position out of range')
  else:
    Newa = [0] * (n-1) #if Newa = [] * (n-1)(read Gemini that know why difference)
    for i in range (0,position-1 + 1,1):
      Newa[i] = a[i] # if Newa = [] * (n-1) -> Newa.append(a[i])(read Gemini that know why difference)
    for i in range (position,n-2 + 1,1):
      Newa[i] = a[i+1]# if Newa = [] * (n-1) -> Newa.append(a[i+1]) (read Gemini that know why difference)
    print ('Array after deletion: ')
    PrintArray (n-1,Newa)


def PrintArray (n,a):
  for i in range (0,n-1 + 1,1):
    print (a[i])

print ('Given an integer n, create an array with n random integers from 0 to 9. Delete an element at the given position in the array.')
print ('Enter number n = ')
n = int (input ())
a = []  # Initialize a as an empty list
for i in range (0,n-1 + 1,1):
  a.append(rd.randint (0,9))
print ('Original array: ')
PrintArray (n,a)
print ('Enter the position of the element to be deleted: ')
position = int (input ())
DeleteElement (n,position,a)
