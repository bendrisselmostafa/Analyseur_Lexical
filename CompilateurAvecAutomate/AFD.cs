using System;
using System.Collections.Generic;
using System.Text;

namespace CompilateurAvecAutomate
{
    class AFD
    {
        int nbrEtat;
        List<char> alphabet;
        int etatInitial;
        int nbrEtatFinal;
        List<int> etatFinaux;
        int nbrTransitions;
        List<Transition> transitions;

        public void setNbrEtat(int nE)
        {
            nbrEtat = nE;
        }
        public int getNbrEtat()
        {
            return nbrEtat;
        }
        public void setAlphabet(List<char> Alph)
        {
            alphabet = Alph;
        }
        public List<char> getAlphabet()
        {
            return alphabet;
        }

        public void setEtatInitial(int EI)
        {
            etatInitial = EI;
        }
        public int getEtatInitial()
        {
            return etatInitial;
        }

        public void setNbrEtatFinal(int nF)
        {
            nbrEtatFinal = nF;
        }
        public int getNbrEtatFinal()
        {
            return nbrEtatFinal;
        }

        public void setEtatFinaux(List<int> EF)
        {
            etatFinaux = EF;
        }
        public List<int> getEtatFinaux()
        {
            return etatFinaux;
        }

        public void setNbrTransitions(int NT)
        {
            nbrTransitions = NT;
        }
        public int getNbrTransitions()
        {
            return nbrTransitions;
        }

        public void setTransitions(List<Transition> T)
        {
            transitions = T;
        }
        public List<Transition> getTransitions()
        {
            return transitions;
        }
        
        

      
    }
}
