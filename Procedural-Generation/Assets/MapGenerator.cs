using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*les variables publiques sont visibles et accessibles dans l'inspecteur*/

public class MapGenerator : MonoBehaviour
{

    /* matrice d'entier de 1 et de 0:
     * 1 pour representer un mur
     * et 0 pour l'abscence de mur 
     */
    int[,] map = null;

    /* pour decider a quel point on la rempli de mur*/
    [Range(0, 100)]
    public int randomFillPercent;

    /*largeur et hauteur de la matrice*/
    public int width, height;

    /*graine pour faire de l'al�atoire*/
    public string seed;

    /*pour savoir si la seed est utilis�*/
    public bool useRandomSeed;


    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            GenerateMap();
        }
    }

    /*reserve de l'espace pour la matrice*/
    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap(); 

        for (int i = 0; i < 5; i++) {
            SmoothMap();
        }
    }

    /* genere/initialise la matrice de fa�on aleatoire � partir de la seed*/
    void RandomFillMap()
    {
        if (useRandomSeed) //met a jour la graine 
        {
            seed = Time.time.ToString();
        }

        //genere un nombre aleatoire � partir de la seed
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        //parcours de la matrice
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //on veut que les bord de la matrice soit des murs 
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    //pseudoRandom est born�e entre 0 et 100 : s'il est inferieur au % de mur dans la map alors map[x,y]
                    //vaudra 1, 0 sinon
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    /* Pour modifier la valeur d'une cellule en fonction du nombre de mur voisin:
     * si >4 alors on change la cellule en mur, si <4 alors ce n'est pas un mur.
     */
    void SmoothMap()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int murVoisin = GetSourroundingWalls(x, y);
                if(murVoisin > 4)
                {
                    map[x, y] = 1;
                }
                else if (murVoisin < 4)
                {
                    map[x, y] = 0;
                }
            }
        }

    }

    /*Renvoi le nb de mur autour de la cellule en position (x,y)*/
    int GetSourroundingWalls(int posX, int posY)
    {
        int wallCount = 0;

        //parcour 3 par 3 
        for (int i = posX - 1; i <= posX + 1; i++)
        {
            for (int j = posY - 1; j <= posY + 1; j++)
            {
                if (i >= 0 && i < width && j >= 0 && j < height)
                {
                    if (i != posX || j != posY)
                    {
                        wallCount += map[i, j];
                    }
                }
                //on est dans les bordures
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;

    }

    /*pour afficher le gizmo de l'objet et le rendre visible*/
    private void OnDrawGizmos()
    {
        if (map != null)
        { 
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white; //couleur
                    Vector3 pos = new Vector3(-width/2 + x + .5f, 0, -height/2 + y +.5f);  
                    Gizmos.DrawCube(pos,Vector3.one);
                }
            }
        }
    }

}
