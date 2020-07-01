using System;
using System.Collections.Generic;
using System.Text;

namespace CompilateurAvecAutomate
{
    class Transition
    {
        int etatI;
        char symbole;
        int etatF;
        public void setEtatI(int EI)
        {
            etatI = EI;
        }
        public int getEtatI()
        {
            return etatI;
        }
        public void setSymbole(char S)
        {
            symbole = S;
        }
        public char getSymbole()
        {
            return symbole;
        }
        public void setEtatF(int EF)
        {
            etatF = EF;
        }
        public int getEtatF()
        {
            return etatF;
        }

    }
}
