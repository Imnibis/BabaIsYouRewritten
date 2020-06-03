using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsText : MonoBehaviour {

    // Instance d'item associé à ce "IS"
    ItemInstance itemInstance;

    Item verticalItem;
    Effector verticalEffector;
    Item horizontalItem;
    Effector horizontalEffector;

    // Lorsque le component est créé
    private void Start()
    {
        // On récupère l'instance d'item de l'objet
        itemInstance = GetComponent<ItemInstance>();
    }

    // Vérifier si "IS" est entouré par du texte
    public void CheckIfSurrounded()
    {
        // Quatres variables où les stocker
        ItemInstance topText = null;
        ItemInstance bottomText = null;
        ItemInstance leftText = null;
        ItemInstance rightText = null;

        // Pour chaque texte dans le ComponentsRegistry
        foreach(ItemInstance item in ComponentsRegistry.instance.GetInstancesFromItemName("text"))
        {
            // Si le texte est sur la même colonne de la grille
            if(item.gridPosition.y == itemInstance.gridPosition.y && !item.GetComponent<IsText>())
            {
                // Si le texte est à la gauche du "IS"
                if (item.gridPosition.x + 1 == itemInstance.gridPosition.x && item.GetComponent<Item>() != null)
                    leftText = item; // On le met dans la variable
                // Sinon, si le texte est à la droite du "IS"
                else if (item.gridPosition.x - 1 == itemInstance.gridPosition.x)
                    rightText = item; // On le met dans la variable
            }
            // Sinon, si le texte est sur la même ligne de la grille
            else if(item.gridPosition.x == itemInstance.gridPosition.x && !item.GetComponent<IsText>())
            {
                // Si le texte est au dessus du "IS"
                if (item.gridPosition.y - 1 == itemInstance.gridPosition.y && item.GetComponent<Item>() != null)
                {
                    topText = item; // On le met dans la variable
                }
                // Sinon, si le texte est en dessous du "IS"
                else if (item.gridPosition.y + 1 == itemInstance.gridPosition.y)
                {
                    bottomText = item; // On le met dans la variable
                }
            }
        }

        // Si il était entouré verticalement auparavant
        if (verticalEffector != null && verticalItem != null)
        {
            if (topText == null ||
                bottomText == null ||
                verticalItem.itemName != topText.GetComponent<Item>().itemName ||
                verticalEffector.effectorType != bottomText.GetComponent<Effector>().effectorType)
            {
                // On enlève le lien entre item et effecteur et on réinitialise la variable
                Debug.Log("Broken vertical link");

                verticalEffector.UnbindItem(verticalItem);

                verticalEffector = null;
                verticalItem = null;
            }
        }

        // Si "IS" est entouré verticalement
        if (topText != null && bottomText != null)
        {
            DefineRule(topText, bottomText, "vertical"); // On définit une règle avec les deux textes
        }

        // Si il était entouré horizontalement auparavant
        if (horizontalEffector != null && horizontalItem != null)
        {
            if (leftText == null ||
                rightText == null ||
                horizontalItem.itemName != leftText.GetComponent<Item>().itemName ||
                horizontalEffector.effectorType != rightText.GetComponent<Effector>().effectorType)
            {
                // On enlève le lien entre item et effecteur et on réinitialise la variable
                Debug.Log("Broken horizontal link");

                horizontalEffector.UnbindItem(horizontalItem);

                horizontalEffector = null;
                horizontalItem = null;
            }
        }

        // Si "IS" est entouré horizontalement
        if (leftText != null && rightText != null)
        {
            DefineRule(leftText, rightText, "horizontal"); // On définit une règle avec les deux textes
        }
    }

    // Fonction pour définir une règle
    void DefineRule(ItemInstance activator, ItemInstance activated, string direction)
    {
        // Si aucun des deux textes n'est un "IS"
        if (!activator.GetComponent<IsText>() && !activated.GetComponent<IsText>())
        {
            Debug.Log("Defining rule with activating text " + activator.gameObject.name + " and activated text " + activated.gameObject.name);

            // Si le texte activé est un item
            if (activated.GetComponent<Item>() != null)
                ReplaceItem(activator, activated); // On remplace l'item activant par l'item activé
            // Sinon, si le texte activé est un effecteur
            else if (activated.GetComponent<Effector>())
            {
                Effector effector = activated.GetComponent<Effector>();

                // On enregistre les deux textes au cas où la règle est cassée
                if (direction == "vertical")
                {
                    verticalEffector = effector;
                    verticalItem = activator.GetComponent<Item>();
                }
                else
                {
                    horizontalEffector = effector;
                    horizontalItem = activator.GetComponent<Item>();
                }
                
                effector.BindItem(activator.GetComponent<Item>()); // On rattache l'item activant à l'effecteur
            }
            // Sinon
            else
                Debug.LogError("Text must have either an item or an effector attached to it."); // Un de nous deux est un sac et a oublié un component
        }
    }

    // Fonction pour remplacer un item par un autre
    void ReplaceItem(ItemInstance itemText, ItemInstance newItemText)
    {
        Debug.Log("Replace item " + itemText.GetComponent<Item>().itemName + " with " + newItemText.GetComponent<Item>().itemName);
        foreach(ItemInstance inst in ComponentsRegistry.instance.GetInstancesFromItemName(itemText.GetComponent<Item>().itemName))
        {
            Instantiate(newItemText.GetComponent<Item>().itemPrefab, inst.transform.position, Quaternion.Euler(Vector3.zero));
            ComponentsRegistry.instance.DestroyInstance(inst);
        }
    }
}
