import matplotlib.pyplot as plt
import os
import re
import numpy as np

tree = next(os.walk(os.getcwd()))[2]

for path in ['StressTestFirst.txt', 'StressTestSecond.txt', 'StressTestSingle.txt']:

    f = open(path, 'r')
    y = sorted([int(l) for l in f])

    plt.hist(y, color = 'blue', edgecolor = 'black', bins = 25)
    
    # Add labels
    plt.title(path)
    plt.xlabel('Delay ms')
    plt.ylabel('Count')
    
    plt.show()

    f.close()

for test in ['First_', 'Second_', 'Single_']:
    x = {}
    for f in tree:
        if re.search(r'TestFirst_', f) != None:
            y = []
            for l in open(f, 'r'):
                y.append(int(l))
            x[int(re.search(r'\d+', f).group(0))] = [np.median(y), sum(y) / len(y)]


    ymed = []
    yaver = []
    xx = sorted([i for i in x])
    for i in xx:
        ymed.append(x[i][0])
        yaver.append(x[i][1])
    
    plt.plot(xx, ymed, label = 'median')
    plt.plot(xx, yaver, label = 'average')
    plt.legend(loc='upper left', borderaxespad=0.)
    plt.title(test[:-1])
    plt.xlabel('pixels')
    plt.ylabel('Delay ms')
    plt.show()
