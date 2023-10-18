
# Introduction
There are lots of interesting things to consider in the given task


# Problem Defintion

> This following exercise is intended to help aid discussion - there are no wrong answers. You will
have to make assumptions to complete the implementation. Please document your assumptions.
Please limit the scope to about 2 hours (we’re not looking for a full solution – it’s up to you where
you put the focus) and implemented in C#. Document what you have left out of the scope.
Domain
A Truck Plan describes a single driver driving a truck for a continuous period. For example; a five
hour drive through Germany on a specific date. A driver is a person with a name, birthdate, etc.
Each truck has a GPS device installed. This device provides the system with the current truck
position approximately every 5 minutes.
1. Design and implement a model for representing the domain.
2. Implement functionality to calculate the approximate distance driven for a single TruckPlan.
3. Find a way to get the country from a coordinate. A solution could, for example, be to call an
external web service.
4. Implement functionality for answering the following question: "How many kilometers did
drivers over the age of 50 drive in Germany in February 2018?"
We look forward to discussing your solution to this exercise!
Please send us the solution in advance of the interview so we can set it up for your arrival.
1. 

# General Comments

There is the problem of determining if a coordinate is within a polygon or a routesegment intersects with a polygon.

Numerous solutions i.e algorithms can be found on the internet.

Using API call to determine if a coordinate is within a country is ok if asking with one coordinate. But if you need to ask with 1000 coordinates then it might become an issue.

An alternative wwould be to find or make an api that filters a list of coordinates by country. 


# Comments to solution.


* I have tried to record types in order to avoid primitives obsession anti pattern
* I have added some test specifications.
* Naming could be improved
* some code could probably be optimized or solved in a more elegant way. (But time is an issue here)
* Ideally I would like to obtain human readable tests. Some test frameworks helps here. But alosnaming classes and putting test files into sub forlder can help readability.
* I havent used layers here. One could use port and adapters/ clean architecture. My self would promote slicebased architecture. It depends there are pros and cons. And there are fiex for the cons. And sometimes one can live with the cons.
















