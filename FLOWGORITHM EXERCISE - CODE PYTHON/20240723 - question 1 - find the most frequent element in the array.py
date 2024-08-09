import random as rd
count = list
def MostFrequentElement (n,a):
  count = [1] * n  # Initialize count list with 1s for each element
  for i in range (0,n-1 + 1,1):
    for j in range (0,n-1 + 1,1):
      if i != j and a[i] == a[j]:  # Corrected condition for counting
        count[i] = count[i]+1
  MaxFrequent = 0  # Initialize MaxFrequent within the function
  for i in range (0,n-1 + 1,1):
    if (count[i] > count[MaxFrequent]):
      MaxFrequent = i
  print ('The most frequent element of the array is: ' + str(a[MaxFrequent]) + ' with ' + str(count[MaxFrequent]) + ' occurrences')  # Use + for string concatenation

def PrintArray (n,a):
  for i in range (0,n-1 + 1,1):
    print (a[i])

print ('Given an integer n, create an array with n random integers from 0 to 9. Find the most frequent element in the array.')
print ('Enter number n = ')
n = int (input ())
a = []  # Initialize a as an empty list
for i in range (0,n-1 + 1,1):
  a.append(rd.randint (0,9))
print ('The element of the array: ' )
PrintArray (n,a)
MostFrequentElement (n,a)
