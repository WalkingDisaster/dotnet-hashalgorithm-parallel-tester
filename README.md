# Hash Algorithm Paralel Tester 

Pretty much does what it says.

Here's an example from my run results. The "Gen{n}" columns represent the number of times that garbage collection ocurred in that generation for the test. 

| Name                | TotalTimeInMilliseconds | IterationsRun | AverageMillisecondsPerRun | Gen0 | Gen1 | Gen2 |
| ------------------- | ----------------------- | ------------- | ------------------------- | ---- | ---- | ---- |
| Static Method       | 3502                    | 10000000      | 0.0003502                 | 385  | 2    | 1    |
| Thread Static       | 10129                   | 10000000      | 0.0010129                 | 618  | 2    | 1    |
| Transient Instances | 70350                   | 10000000      | 0.007035                  | 1318 | 69   | 7    |
| Singleton Instance  | 743777                  | 10000000      | 0.0743777                 | 1237 | 63   | 14   |