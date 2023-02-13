//Inicio do jogo da velha

string[,] matriz = new string[3, 3] { 
    { "null", "null", "null" },
    { "null", "null", "null" },
    { "null", "null", "null" } };
bool jogoRolando = true;
int jogador = 1;

while (jogoRolando)
{
    PrintarJogo();
    JogaPlayer(jogador);
    if (VerificarGanhador(jogador))
    {
        PrintarJogo();
        Console.WriteLine(string.Format("Jogador {0}({1}) venceu!!!", jogador, Enum.GetName(typeof(Simbolos), jogador)));
        jogoRolando = false;
    }
    jogador = (jogador + 1) % 2;

}
void PrintarJogo()
{
    int rowLength = matriz.GetLength(0);
    int colLength = matriz.GetLength(1);

    for (int i = 0; i < rowLength; i++)
    {
        for (int j = 0; j < colLength; j++)
        {
            Console.Write(string.Format("{0} ", matriz[i, j] + " | "));
        }
        Console.Write(Environment.NewLine + Environment.NewLine);
    }
};
void JogaPlayer(int jogador)
{
    var JogadorSimbolo = Enum.GetName(typeof(Simbolos), jogador);
    Console.WriteLine(string.Format("Jogador ({0}) Digite a coluna selecionada: ",JogadorSimbolo));
    var coluna = Convert.ToInt16(Console.ReadLine()) -1;
    if (coluna < 0 || coluna > 2)
    {
        Console.WriteLine("Coluna invalida");
        JogaPlayer(jogador);
        return;
    }
    Console.WriteLine(string.Format("Jogador ({0}) Digite a linha selecionada: ", JogadorSimbolo));
    var linha = Convert.ToInt16(Console.ReadLine()) -1;
    if( linha < 0 || linha > 2)
    {
        Console.WriteLine("Linha invalida");
        JogaPlayer(jogador);
        return;
    }

    if (matriz[linha,coluna] == "null")
    {
        matriz[linha, coluna] = Enum.GetName(typeof(Simbolos), jogador);
    }
    else
    {
        Console.WriteLine("Posição já foi escolhido por um jogador");
        JogaPlayer(jogador);
        return;
    }
}
bool VerificarGanhador(int jogador)
{
    if (VerificarGanhadorHorizontal(jogador) || VerificarGanhadorVertical(jogador) || VerificarGanhadorDiagonal(jogador))
    {
        return true;
    }
    else return false;
}
bool VerificarGanhadorHorizontal (int jogador)
{
    bool jogadorGanhou = false;
    var jogadorSimbolo = Enum.GetName(typeof(Simbolos), jogador);
    for (var linha = 0; linha < 3; linha++)
    {
        if (matriz[linha, 0] == jogadorSimbolo && matriz[linha, 1] == jogadorSimbolo && matriz[linha, 2] == jogadorSimbolo)
        {
            jogadorGanhou = true;
        }
    }
    return jogadorGanhou;
}
bool VerificarGanhadorVertical(int jogador)
{
    bool jogadorGanhou = false;
    var jogadorSimbolo = Enum.GetName(typeof(Simbolos), jogador);
    for (var coluna = 0; coluna < 3; coluna++)
    {
        if (matriz[0, coluna] == jogadorSimbolo && matriz[1, coluna] == jogadorSimbolo && matriz[2, coluna] == jogadorSimbolo)
        {
            jogadorGanhou = true;
        }
    }
    return jogadorGanhou;
}
bool VerificarGanhadorDiagonal(int jogador)
{
    bool jogadorGanhou = false;
    var jogadorSimbolo = Enum.GetName(typeof(Simbolos), jogador);
    if (matriz[0, 0] == jogadorSimbolo && matriz[1, 1] == jogadorSimbolo && matriz[2, 2] == jogadorSimbolo)
    {
        jogadorGanhou = true;
    }
    if (matriz[0, 2] == jogadorSimbolo && matriz[1, 1] == jogadorSimbolo && matriz[2, 0] == jogadorSimbolo)
    {
        jogadorGanhou = true;
    }

    return jogadorGanhou;
}
enum Simbolos
{
    O,
    X
};