

var jogo = new Jogo();
var tabuleiro = new Tabuleiro();
var jogador = new JogadorHumano();
var jogadorRandom = new RandomPlayer();
jogo.Joga(tabuleiro, jogador, jogadorRandom);

class Jogo
{
    static int[,] PosicaoDeVitoria = {  { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
                                        { 0, 4, 8 }, { 6, 4, 2 }, { 6, 3, 9 },
                                        { 7, 4, 1 }, { 8, 5, 2 }};
    public bool VerificarGanhador(Tabuleiro Tabuleiro)
    {
        for (int i = 0; i < PosicaoDeVitoria.GetLength(0); i++)
        {
            if (Tabuleiro.tabuleiro[PosicaoDeVitoria[i, 0]] == Tabuleiro.tabuleiro[PosicaoDeVitoria[i, 1]] && Tabuleiro.tabuleiro[PosicaoDeVitoria[i, 1]] == Tabuleiro.tabuleiro[PosicaoDeVitoria[i, 2]] &&
                Tabuleiro.tabuleiro[PosicaoDeVitoria[i, 0]] != Tabuleiro.Vazio && Tabuleiro.tabuleiro[PosicaoDeVitoria[i, 1]] != Tabuleiro.Vazio && Tabuleiro.tabuleiro[PosicaoDeVitoria[i, 2]] != Tabuleiro.Vazio)
                return true;
        }
        return false;
    }
    public void Joga(Tabuleiro tabuleiro1,JogadorHumano jogadorHumano,RandomPlayer randomPlayer)
    {
        bool jogoRolando = true;
        int jogador = 1;
        while (jogoRolando)
        {
            if (jogador == 1)
            {
                tabuleiro1.PrintTabuleiro();
                jogadorHumano.Joga(tabuleiro1,jogador);
            }
            else
                randomPlayer.JogaRandom(tabuleiro1,jogador);
            if (VerificarGanhador(tabuleiro1))
            {
                Console.WriteLine(tabuleiro1.PrintTabuleiro());
                Console.WriteLine(string.Format("Jogador {0}({1}) venceu!!!", jogador, Enum.GetName(typeof(Tabuleiro.Simbolos), jogador)));
                jogoRolando = false;
            }
            jogador = (jogador + 1) % 2;

        }
    }
}

class Tabuleiro
{
    public enum Simbolos
    {
        O,
        X
    };
    static public string Vazio = " ";
    static public string[] tabuleiro = { Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio };
    public string PrintTabuleiro()
    {
        int tabuleiroLength = tabuleiro.Length;

        var tabuleiroprint = string.Format(" {0} | {1} | {2} \n", tabuleiro[6], tabuleiro[7], tabuleiro[8]);
        tabuleiroprint += "---+---+---\n";
        tabuleiroprint += string.Format(" {0} | {1} | {2} \n", tabuleiro[3], tabuleiro[4], tabuleiro[5]);
        tabuleiroprint += "---+---+---\n";
        tabuleiroprint += string.Format(" {0} | {1} | {2} \n", tabuleiro[0], tabuleiro[1], tabuleiro[2]);
        return tabuleiroprint;
    }
    public void Joga(int jogador, int posicaoJogada)
    {
        try
        {
            if (Tabuleiro.tabuleiro[posicaoJogada] != Vazio)
            {
                Console.WriteLine("Jogada ja foi feita por outro jogador.");
                Console.WriteLine("Posicoes disponiveis" + GetJogadasPossiveisString().ToArray().ToString());
                return;
            }
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Posicao invalida");
            Console.WriteLine("Posicoes disponiveis" + GetJogadasPossiveisString().ToArray().ToString());
            return;
        }
        tabuleiro[posicaoJogada] = Enum.GetName(typeof(Simbolos), jogador);
    }
    public List<int> GetJogadasPossiveis()
    {
        var listaJogadasLivres = new List<int>();
        for (int i = 0; i < tabuleiro.Length; i++)
        {
            if (tabuleiro[i] == Vazio)
            {
                listaJogadasLivres.Add(i);
            }
        }
        return listaJogadasLivres;
    }
    public List<int> GetJogadasPossiveisString()
    {
        var listaJogadasLivres = new List<int>();
        for (int i = 0; i < tabuleiro.Length; i++)
        {
            if (tabuleiro[i] == Vazio)
            {
                listaJogadasLivres.Add(i+ 1);
            }
        }
        return listaJogadasLivres;
    }
}
class JogadorHumano
{
    public void Joga(Tabuleiro tabuleiro, int jogador)
    {
        int posicaoJogada;
        while (true)
        {
            var JogadorSimbolo = Enum.GetName(typeof(Tabuleiro.Simbolos), jogador);
            Console.WriteLine(string.Format("Jogador ({0}) Digite a posicao para jogar: ", JogadorSimbolo));
            posicaoJogada = Convert.ToInt16(Console.ReadLine()) - 1;
            if (tabuleiro.GetJogadasPossiveis().Contains(posicaoJogada))
            {
                break;
            }
        }
        tabuleiro.Joga(jogador, posicaoJogada);
    }
}
class RandomPlayer
{
    public void JogaRandom(Tabuleiro tabuleiro,int jogador)
    {
        var jogadasAbertas = tabuleiro.GetJogadasPossiveis();
        Random rnd = new Random();
        int r = rnd.Next(jogadasAbertas.Count);
        tabuleiro.Joga(jogador, jogadasAbertas[r]);
    }

}
