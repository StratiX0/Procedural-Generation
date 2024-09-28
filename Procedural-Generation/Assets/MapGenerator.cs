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

    /* pour remplir la map de facon aléatoire
     * et decider a quel point on la rempli de mur
     */
    [Range(0,100)]
    public int randomFillPercent;

    /*largeur et hauteur de la map*/
    public int width, height;

    /*graine pour faire de l'aléatoire*/
    public string seed;

    /*pour savoir si la seed est utilisé???*/
    public bool useRandomSeed;


    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    /*reserve de l'espace pour la matrice*/
    void GenerateMap()
    {
        map = new int[width,height];
        
    }

    /*genere/initialise la matrice de façon aleatoire à partir d'une graine */
    void RandomFillMap()
    {
        if (useRandomSeed) //met a jour la graine 
        {
            seed = Time.time.ToString();
        }

        //genere un nombre aleatoire à partir de la seed
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        //parcours de la matrice
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                //pseudoRandom est borner entre 0 et 100 si il est inferieur au % de mur dans la map alors map[x,y] vaudra 1,
                //0sinon
                map[x, y] = pseudoRandom.Next(0, 100) < randomFillPercent ? 1 : 0;
            }
        }

    }


    /**/
    private void OnDrawGizmos()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {

            }
        }
    }

}
