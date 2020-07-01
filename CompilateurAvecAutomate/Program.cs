using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Dynamic;
using System.ComponentModel;
using System.Threading;

namespace CompilateurAvecAutomate
{
    class Program
    {
        // TABLEAU DE STRING OU ON INSERE LES MOTS-CLE DEFINIT DANS LE CAHIER DE CHARGES
        static string[] MOTCLE = { "IF", "THEN", "ELSE", "BEGIN", "END" };
        
        //LA FONCTION PRINT QUI PERMET D'AFFICHER L'AUTOMATE AINSI QUE CES COMPOSANTS
        public static void print(AFD AUTOM)
        {
            // BOUCLE QUI PERMET D'AFFICHER LES DIFFERENTS ETATS (INITIAUX ET FINAUX)
            for (int i = 0; i < AUTOM.getNbrEtat(); i++)
            {
                if (i == 0)
                    Console.Write("E=({0}", i); // LE PREMIER ETAT 
                else
                    if (i == AUTOM.getNbrEtat() - 1) 
                    Console.WriteLine(",{0})", i); // LA DERNIERE ETAT
                else
                    Console.Write(",{0}", i); // LES AUTRES ETATS DU MILIEU 

            }
            // BOUCLE QUI PERMET D'AFFICHER L'ALPHABET DE L'AUTOMATE
            for (int i = 0; i < AUTOM.getAlphabet().Count; i++)
            {
                if (i == 0)
                    Console.Write("A=({0}", AUTOM.getAlphabet()[i]); // LE PREMIER CARACTERE 
                else
                    if (i == AUTOM.getAlphabet().Count - 1)
                    Console.WriteLine(",{0})", AUTOM.getAlphabet()[i]); // LE DERNIER CARACTERE 
                else
                    Console.Write(",{0}", AUTOM.getAlphabet()[i]); // LES AUTRES CARACTERES DE L'ALPHABET 

            }
            Console.WriteLine("Transactions:");
            // BOUCLE QUI PERMET D'AFFICHER LES TRANSACTIONS
            for (int i = 0; i < AUTOM.getTransitions().Count; i++)
            {
                Console.WriteLine("t({0},{1})={2}  ", AUTOM.getTransitions()[i].getEtatI(), AUTOM.getTransitions()[i].getSymbole(), AUTOM.getTransitions()[i].getEtatF());
            }
            
            // L'AFFICHAGE DE L'ETAT INITIAL
            Console.WriteLine("I= {0}", AUTOM.getEtatInitial());
            // L'AFFICHAGE DES ETATS FINAUX
            if (AUTOM.getNbrEtatFinal() == 1)
                Console.WriteLine("F=({0})", AUTOM.getEtatFinaux()[0]); // SI IL S'AGIT D'UN SEUL ETAT FINAL
            else
                for (int i = 0; i < AUTOM.getNbrEtatFinal(); i++)
                {
                    if (i == 0)
                        Console.Write("F=( {0}", AUTOM.getEtatFinaux()[i]); // LE PREMIER ETAT FINAL
                    else
                        if (i == AUTOM.getNbrEtatFinal() - 1)
                        Console.WriteLine(", {0} )", AUTOM.getEtatFinaux()[i]); // LE DERNIER ETAT FINAL
                    else
                        Console.Write(", {0}", AUTOM.getEtatFinaux()[i]); // LES AUTRES ETATS FINAUX

                }
            // L'AFFICHAGE DE NOMBRE DES TRANSACTIONS
            Console.WriteLine("Nombre de transactions : {0}", AUTOM.getTransitions().Count);
        }

        //LA FONCTION PRINT QUI PERMET DE LIRE UN AUTOMATE DEPUIS UN FICHIER TEXTE
        public static AFD read(string chemin)
        {
            int j; // COMPTEUR QUI RECOIT LES CARACTERE ASCII EN INT
            char c; // CARACTERE QUI RECOIT LE SYMBOLE D'UNE TRANSITION
            AFD AUTOM = new AFD(); // L'AFD A REMPLIR ET A RETOURNER

            // LECTURE DU LA CHIAINE OU SE TROUVE L'AUTOMATE
            using (StreamReader sr = new StreamReader(chemin))
            {
                
                string ligne; // LA CHAINE QUI REPRENSTERA LE FICHIER AFD
                int curs = 1; // CURSEUR QUI DEFINIRAIT A QUOI CORRESPOND CHAQUE LIGNE 
                List<char> Alphabet = new List<char>(); // LISTE DE CARACTERES DE L'ALPHABET
                AUTOM.setAlphabet(Alphabet); // ALLOUER A L'ATTRIBUT ALPHABET UN ESPACE MEMOIRE CREE PAR LA LISTE ALPHABET
                List<int> EFinaux = new List<int>(); // LISTE D'ENTIERS DES ETATS FINAUX DANS LAQUELLE ON VA INSERER NOS ETATS FINAUX
                List<Transition> Tr = new List<Transition>(); // LISTE DE TRANSITION OU ON VA AJOUTER CHAQUE FOIS ON CREE UNE NOUVELLE TRANSITION 
                Transition t = new Transition(); // INTACIATION D'UN OBJET DE TYPE TRANSITION 
                while ((ligne = sr.ReadLine()) != null)
                {
                    // RENITIALISATION DE L'OBJET TRANSITION ...
                    t = new Transition();
                    switch (curs)
                    {
                        // REMPLISSAGE DE L'ATTRIBUT nbrEtat 
                        case 1:
                            AUTOM.setNbrEtat(Int16.Parse(ligne));
                            break;
                        // REMPLISSAGE DE L'ALPHABET DE L'AFD 
                        case 2:
                            // SEPARATION DES CARACTERE DE L'ALPHABET SACHANT QUE LES CARACTERE SONT SEPARE PAR DES ESPACES 
                            // (ON PEUT LA MODIFIER SI ON VEUT QUE L'ALPHABET SOIT UNE SEULE CHAINE AVEC AUCUN ESPACE ENTRE LES CARACTERE DE L'ALPHABET)
                            string[] alph = ligne.Split(" "); 
                           
                            // BOUCLE QUI SE REPETE TANT QU'ON TROUVE DES ELEMENTS DANS LE TABLEAU D'ALPHABET 
                            foreach (string al in alph)
                            {
                                AUTOM.getAlphabet().Add(char.Parse(al));
                            }
                            break;
                        // REMPLISSAGE DE L'ATTRIBUT etatInitial
                        case 3:
                            AUTOM.setEtatInitial(Int16.Parse(ligne));
                            break;
                        // REMPLISSAGE DE L'ATTRIBUT nbrEtatFinal 
                        case 4:
                            AUTOM.setNbrEtatFinal(Int16.Parse(ligne));
                            break;
                        // REMPLISSAGE DE LA LISTE EFINAUX QUI CONTIENDERA L'ENSEMBLE D'ETAT FINAUX 
                        case 5:
                            string[] EF = ligne.Split(" ");
                            foreach (string etat in EF)
                            {
                                EFinaux.Add(Int16.Parse(etat));
                            }
                            break;
                        // REMPLISSAGE DE LA LISTE DES TRANSACTIONS 
                        default:
                            string[] trasac = ligne.Split(" ");
                            c = char.Parse(trasac[1]);
                            // LE SYMBLOE DE LA TRANSITION EST C --> CHIFFRE
                            if (c == 'C') 
                            {
                                // BOUCLE QUI PARCOURS LES CODES ASCII DES CHIFFRES --> DE 0 à 9
                                for (j = 48; j <= 57; j++) 
                                {
                                    // DEFINITION D'UN NOUVEAU OBJET TRANSITION AFIN DE LUI AFFECTER LA NOUVELLE TRANSITION AVEC DES NOUVELLES VALEURS POUR LES ATTRIBUTS ( etatI - symbole - etatF )
                                    t = new Transition();
                                    t.setEtatI(Int16.Parse(trasac[0]));
                                    t.setSymbole((char)j);
                                    t.setEtatF(Int16.Parse(trasac[2]));
                                    // AJOUTER LA NOUVELLE TRANSITION A LA LISTE DE TRANSITIONS
                                    Tr.Add(t);
                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST L --> LETTRE
                            if (c == 'L')
                            {
                                // BOUCLE QUI PARCOURS LES CODES ASCII DES LETTRE MAJ --> DE A à Z
                                for (j = 65; j <= 90; j++) 
                                {
                                    t = new Transition();
                                    t.setEtatI(Int16.Parse(trasac[0]));
                                    t.setSymbole((char)j);
                                    t.setEtatF(Int16.Parse(trasac[2]));
                                    Tr.Add(t);
                                }
                                // BOUCLE QUI PARCOURS LES CODES ASCII DES LETTRE MIN --> DE a à z
                                for (j = 97; j <= 122; j++) 
                                {
                                    t = new Transition();
                                    t.setEtatI(Int16.Parse(trasac[0]));
                                    t.setSymbole((char)j);
                                    t.setEtatF(Int16.Parse(trasac[2]));
                                    Tr.Add(t);
                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST X --> IL S'AGIT DE N'IMPORTE QUOI DE L'AUTOMATE QUI LIT LES CHAINES DE CARACTERES
                            if (c == 'X')
                            {
                                // BOUCLE QUI PARCOURS LES CODES ASCII DE TOUT LE CLAVIER
                                for (j = 32; j <= 126; j++)  
                                {
                                    // SAUF LE CARACTERE " CAR IL Y A UNE TRANSITION UNIQUE AVEC CE SYMBOLE QUI PERMETTERA D'ALLER VERS UN ETAT FINAL AINSI DE SORTIR DE L'AUTOMATE
                                    if (j != 34)
                                    {
                                        t = new Transition();
                                        t.setEtatI(Int16.Parse(trasac[0]));
                                        t.setSymbole((char)j);
                                        t.setEtatF(Int16.Parse(trasac[2]));
                                        Tr.Add(t);
                                    }

                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST 1 --> PREMIER AUTRE, IL S'AGIT DU CODER TOUT LE CAS POSSIBLE DU CLAVIER POUR ALLER VERS UN ETAT FINAL
                            if (c == '1')
                            {
                                for (j = 32; j <= 126; j++)
                                {
                                    // SAUF LE = 
                                    if (j != 61)
                                    {
                                        t = new Transition();
                                        t.setEtatI(Int16.Parse(trasac[0]));
                                        t.setSymbole((char)j);
                                        t.setEtatF(Int16.Parse(trasac[2]));
                                        Tr.Add(t);
                                    }

                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST 2 --> DEUXIEME AUTRE, IL S'AGIT DU CODER TOUT LE CAS POSSIBLE DU CLAVIER POUR ALLER VERS UN ETAT FINAL
                            if (c == '2')// autre 2
                            {
                                for (j = 32; j <= 126; j++)
                                {
                                    // AUTRE: DIFFERENT DE = ET >
                                    if (j != 61 && j != 62)
                                    {
                                        t = new Transition();
                                        t.setEtatI(Int16.Parse(trasac[0]));
                                        t.setSymbole((char)j);
                                        t.setEtatF(Int16.Parse(trasac[2]));
                                        Tr.Add(t);
                                    }

                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST 3 --> TROISIEME AUTRE, IL S'AGIT DU CODER TOUT LE CAS POSSIBLE DU CLAVIER POUR ALLER VERS UN ETAT FINAL
                            if (c == '3')
                            {
                                for (j = 32; j <= 126; j++)
                                {
                                    // AUTRE : DIFFERENT DES LETTRES ( MAJ- MIN ) ET LES CHIIFRES (0 à 9)
                                    if ((j < 48) || (j > 57 && j < 65) || (j > 90 && j < 97) || (j > 122))
                                    {
                                        t = new Transition();
                                        t.setEtatI(Int16.Parse(trasac[0]));
                                        t.setSymbole((char)j);
                                        t.setEtatF(Int16.Parse(trasac[2]));
                                        Tr.Add(t);
                                    }

                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST 4 --> QUATRIEME AUTRE, IL S'AGIT DU CODER TOUT LE CAS POSSIBLE DU CLAVIER POUR ALLER VERS UN ETAT FINAL
                            if (c == '4')
                            {
                                for (j = 32; j <= 126; j++)
                                {
                                    // AUTRE : DIFFERENT DES CHIFFES ET DU .    => POUR LIRE SEUELEMENT LES ENTIERS
                                    if ((j < 48 || j > 57) && j != 46)
                                    {
                                        t = new Transition();
                                        t.setEtatI(Int16.Parse(trasac[0]));
                                        t.setSymbole((char)j);
                                        t.setEtatF(Int16.Parse(trasac[2]));
                                        Tr.Add(t);
                                    }

                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST 5 --> CINQUEME AUTRE, IL S'AGIT DU CODER TOUT LE CAS POSSIBLE DU CLAVIER POUR ALLER VERS UN ETAT FINAL
                            if (c == '5')
                            {
                                for (j = 32; j <= 126; j++)
                                {
                                    // AUTRE : DIFFERENT DES CHIFFES ET DU CARACTERE 'E'    => POUR LIRE SEULEMENT LES FLOATS 
                                    if ((j < 48 || j > 57) && j != 69)
                                    {
                                        t = new Transition();
                                        t.setEtatI(Int16.Parse(trasac[0]));
                                        t.setSymbole((char)j);
                                        t.setEtatF(Int16.Parse(trasac[2]));
                                        Tr.Add(t);
                                    }

                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST 6 --> SIXIEME AUTRE, IL S'AGIT DU CODER TOUT LE CAS POSSIBLE DU CLAVIER POUR ALLER VERS UN ETAT FINAL
                            if (c == '6')
                            {
                                for (j = 32; j <= 126; j++)
                                {
                                    // AUTRE : DIFFERENT DES CHIFFES                    => POUR LIRE SEULEMENT LES REELS
                                    if ((j < 48 || j > 57))
                                    {
                                        t = new Transition();
                                        t.setEtatI(Int16.Parse(trasac[0]));
                                        t.setSymbole((char)j);
                                        t.setEtatF(Int16.Parse(trasac[2]));
                                        Tr.Add(t);
                                    }

                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST 7 --> SEPTIEME AUTRE, IL S'AGIT DU CODER TOUT LE CAS POSSIBLE DU CLAVIER POUR ALLER VERS UN ETAT FINAL
                            if (c == '7')
                            {
                                for (j = 32; j <= 126; j++)
                                {
                                    // AUTRE : SAUF )                               => POUR LIRE UN COMMENTAIRE
                                    if (j != 41)
                                    {
                                        t = new Transition();
                                        t.setEtatI(Int16.Parse(trasac[0]));
                                        t.setSymbole((char)j);
                                        t.setEtatF(Int16.Parse(trasac[2]));
                                        Tr.Add(t);
                                    }

                                }
                            }
                            // LE SYMBLOE DE LA TRANSITION EST N --> IL S'AGIT DE N'IMPORTE QUOI DE L'AUTOMATE QUI LIT LES CHAINES DE CARACTERES
                            if (c == 'N')
                            {
                                for (j = 32; j <= 126; j++)
                                {
                                    // SAUF LE CARACTERE * CAR IL Y A UNE TRANSITION UNIQUE AVEC CE SYMBOLE QUI PERMETTERA D'ALLER VERS UN ETAT FINAL AINSI DE SORTIR DE L'AUTOMATE
                                    if (j != 42)
                                    {
                                        t = new Transition();
                                        t.setEtatI(Int16.Parse(trasac[0]));
                                        t.setSymbole((char)j);
                                        t.setEtatF(Int16.Parse(trasac[2]));
                                        Tr.Add(t);
                                    }

                                }
                            }
                            // TOUT AUTRE SYMBOLE SERAIT DEFINIT COMME UNE TRANSACTION UNIQUE DE CE SYMBOLE ENTRE UN ETAT INITIAL ET FINAL
                            if (c != 'L' && c != 'C' && c != 'X' && c != '1' && c != '2' && c != '3' && c != '4' && c != '5' && c != '6' && c != 'N' && c != '7')
                            {
                                t = new Transition();
                                t.setEtatI(Int16.Parse(trasac[0]));
                                t.setSymbole(c);
                                t.setEtatF(Int16.Parse(trasac[2]));
                                Tr.Add(t);

                            }




                            break;
                    }
                    // INCREMENTATION DU CURSEUR QUI INDIQUE LA LIGNE 
                    curs++;
                }
                AUTOM.setEtatFinaux(EFinaux); // REMPLISSAGE DE L'ATTRIBUR etatFinaux AVEC LA LISTE DEJA REMPLIE EFinaux
                AUTOM.setTransitions(Tr); // REMPLISSAGE DE L'ATTRIBUR transitions AVEC LA LISTE DEJA REMPLIE Tr
            }
            return AUTOM; // ON RETOURNE L'AUTOMATE COMPLET 

        }

        // LA FONCTION TRANSITION QUI PERMET DE CHANGER L'ETAT DE L'AUTOMATE SELON UNE TRANSTION UNE ETAT INITIAL ET UN SYMBOLE LU
        public static int transition(int etat, char c, AFD AUTOM)
        {
            // BOUCLE QUI SE REPETERA AUTANT QUE LE NOMBRE DE TRANSITIONS 
            for (int i = 0; i < AUTOM.getTransitions().Count; i++)
            {
                // ON TESTE SI IL' Y A UNE TRANSITION AVEC COMME ETAT INITIAL LE PREMIER PARAMETRE ET LE DEUXIEME COMME SYMBOLE
                if (AUTOM.getTransitions()[i].getEtatI() == etat && AUTOM.getTransitions()[i].getSymbole() == c)
                    return AUTOM.getTransitions()[i].getEtatF(); // SI OUI ON RETOURNE L'ETAT FINAL DE LA TRANSITION

            }
            // SI ON NE SORT PAS DE LA BOUCLE AVEC UN ETAT ON RETOURNE L'ETAT INITIAL
            return etat;
        }

        // LA FONCTION ACCEPT QUI PERMET DE D'AFFICHER LA NATURE DE L'UNITE LEXICAL
        public static void accept(AFD AUTOM, string w)
        {
            int cpt = 0; // COMPTEUR SUR LE CARACTERE DU FICHIER A INTERPRETER
            string mot = ""; // CHAINE QUI CONTIENDERA LA VALEUR DU LEXEME
            int q = AUTOM.getEtatInitial(); // VARIABLE QUI DEFINIT L'ETAT
            // BOUCLE QUI PARCOUR LA CHAINE CONTENANT LE FICHIER A INTERPRETER 
            while (w.Length != cpt)
            {
                q = transition(q, w[cpt], AUTOM); // APPEL DE LA FONCTION TRANSITION AVEC L'ETAT INITAL q ET LE SYMBOLE 
                // ON TEST SI L'ETAT RETOURNER EXISTE DANS LE LA LISTE D'ETAT FINAUX DE L'AFD
                if (AUTOM.getEtatFinaux().Contains(q))
                {
                    switch (q)
                    {
                        // OPERATEUR RELATIONNEL EGAL
                        case 1:
                            Console.WriteLine("<REL,EGAL>");
                            cpt++; // ON INCREMENTE LE COMPLTEUR CAR IL N'Y PAS D'AUTRE QUI INDIQUERAIT LA FIN DE L'UNITE 
                            break;

                        // OPERATEUR RELATIONNEL SUPERIEUR
                        case 15:
                            Console.WriteLine("<REL,SUP>");
                            break;

                        // OPERATEUR RELATIONNEL SUPERIEUR OU EGAL
                        case 16:
                            Console.WriteLine("<REL,SUPEGAL>");
                            cpt++;
                            break;

                        // OPERATEUR RELATIONNEL INFERIEUR
                        case 17:
                            Console.WriteLine("<REL,INF>");
                            break;

                        // OPERATEUR RELATIONNEL INFERIEUR OU EGAL
                        case 18:
                            Console.WriteLine("<REL,INFEGAL>");
                            break;

                        // OPERATEUR RELATIONNEL DIFFERENCE
                        case 19:
                            Console.WriteLine("<REL,DIFF>");
                            cpt++;
                            break;


                        // POUR LES COMMENTAIRES : (**)
                        case 20:
                            cpt++;
                            break;

                        // MOT-CLE OU ID
                        case 21:
                            // ON CHERCHER SI LE MOT RETOURNE EXISTE DANS LA LISTE DE STRING, SI OUI C'EST UN MOT-CLE SINON C'EST UN ID
                            if (MOTCLE.Contains(mot))
                                Console.WriteLine("<MOTCLE,{0}>", mot);
                            else
                                Console.WriteLine("<ID,{0}>", mot);

                            break;

                        // CHAINE DE CARACTERES
                        case 23:
                            Console.WriteLine("<STRING,{0}>", mot + w[cpt++]);
                            break;

                        // ENTIER
                        case 24:
                            Console.WriteLine("<INT,{0}>", mot);
                            break;

                        // OPERATEUR ARITHMETIQUE : + OU - OU * OU /
                        case 25:
                            Console.WriteLine("<OP,{0}>", w[cpt++]);
                            break;

                        // FLOAT
                        case 26:
                            Console.WriteLine("<FLOAT,{0}>", mot);
                            break;

                        // REEL
                        case 27:
                            Console.WriteLine("<REAL,{0}>", mot);
                            break;

                        // UTILISATION DE DEFAULT N'A PAS D'INTERET CAR ON EST SUR QU'ON ENTRE DANS CE TEXTE SEULEMENT SI ON SORT AVEC UN ETAT FINAL LORS D'UNE TRANSITION  
                        default:
                            break;
                    }
                    mot = ""; // RENITIALISATION DU MOT RETOURNE POUR AFFICHIER LE NOUVEAU
                    q = AUTOM.getEtatInitial(); // RENITIALISATION DE L'ETAT INITIAL
                }
                else
                {
                    // REMPLISSAGE DU MOT DE LA VALEUR DU LEXEME, EN NEGLIGEANT LES CARACTERES BLANCS
                    if (w[cpt] != ' ') mot += w[cpt];
                    cpt++; // INCREME?TATION DU COMPTEUR QUI REPRESENTE LE CURSEUR SUR CHAQUE CARACTERE 
                }
                
            }
                                
            
        }

        static void Main(string[] args)
        {
            AFD AUTOM = new AFD { };
            string AFD_txt;
            string Program;

            Console.WriteLine("Entrer le chemin vers le fichier de l'automate: ");
            // C:\Users\DELL\Desktop\automate.txt
            AFD_txt = Console.ReadLine();

            Console.WriteLine("Entrer le chemin vers le fichier a interpreter : ");
            // C: \Users\DELL\Desktop\Program.txt
            Program = Console.ReadLine();
            
            // LECTURE DE L'AUTOMATE
            AUTOM = read(AFD_txt);

            // AFFICHAGE DE L'AUTOMATE
            print(AUTOM);

            // LECTURE DU FICHIER A INTERPRETER, ON LUI AJOUTANT UN ESPACE POUR LA LECTURE DU DERNIER MOT
            string readText = File.ReadAllText(Program) + " ";

            // REMPLACEMENT DES TABULATIONS ET RETOUR CHARIOT (VERTICALE-HORIZENTALE) PAR DES ESPACES BLANCS POUR QU'IL NE RESTE A LA FIN QU'UNE SEUL CHAINE AU LIEU DE PLUSIEURS LIGNES 
            readText = readText.Replace('\r', ' ');
            readText = readText.Replace('\n', ' ');
            readText = readText.Replace('\v', ' ');
            readText = readText.Replace('\t', ' ');

            // APPEL DE LA FONCTION ACCEPT POUR EXTRAIRE TOUT LEXEME DE NOTRE FICHIER TEXTE
            accept(AUTOM, readText);

        }
    }
}