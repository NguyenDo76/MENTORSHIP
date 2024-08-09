import random as rd
count = list
def MostFrequentElement (n,a):
  count = [1] * n
  for i in range (0,n-1 + 1,1):
    for j in range (0,n-1 + 1,1):
      if i != j and a[i] == a[j]:
        count[i] = count[i]+1
  MaxFrequent = 0
  for i in range (0,n-1 + 1,1):
    if (count[i] > count[MaxFrequent]):
      MaxFrequent = i
  print ('The most frequent element of the array is: ' + str(a[MaxFrequent]) + ' with ' + str(count[MaxFrequent]) + ' occurrences')

def PrintArray (n,a):
  for i in range (0,n-1 + 1,1):
    print (a[i])

print ('Given an integer n, create an array with n random integers from 0 to 9. Find the most frequent element in the array.')
print ('Enter number n = ')
n = int (input ())
a = []
for i in range (0,n-1 + 1,1):
  a.append(rd.randint (0,9))
print ('The element of the array: ' )
PrintArray (n,a)
MostFrequentElement (n,a)
