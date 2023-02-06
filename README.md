# MoleculeSimVR

This is a VR application that simulates the behavior of atoms given a dataset; in Unity3D. You can assign different prefabs to different atom elements. 


## Features:

- Set atoms to glow at a specific frame
- Change the emission of the atoms (eg according to their vibration)
- Scale the distances
- Set framer per second
- Asign a prefab to each atom type
- You can move and fly around using any VR controller supported by openAR

![image](https://user-images.githubusercontent.com/8094167/216994457-8f65b221-a866-463a-b1bd-581a6b43b5ce.png)


![aq2](https://user-images.githubusercontent.com/8094167/217001806-5e832ac6-f849-4ef5-9e7d-2a8939d32dc9.gif)





## About Data: 


### Columns

X position, Y position, Z position, emision, unused, unused, Glow

- The emission column is used to make the particle that vibrate the most greener
- The glow column takes 0 as no glow and 1 as glow. In the example is set to 1 when the speed goes above average


###Format

Each line represents the data of one Atom. The information of each line is separated by comma. 
The order of the data is given by caputes or ‘frames’ each frame containing the data of every Atom.
EG:

Frame 1 
Frame 2
... 
Frame n 

 Where Frame 1:

Atom1x,Atom1y,Atom1z,Atom1u,Atom1p
Atom2x,Atom2y,Atom1z,Atom2u,Atom2p
...
AtomNx,AtomNy,AtomNz,AtomNu,AtomNp

![image](https://user-images.githubusercontent.com/8094167/216994241-9d806796-d60b-45db-b2fb-e09656f12b26.png)
