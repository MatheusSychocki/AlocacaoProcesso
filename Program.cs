using System.Collections.Concurrent;

namespace Program
{
    public class Program
    {

        public static ConcurrentQueue<Processo> processosEspera = new ConcurrentQueue<Processo>();
        public static ConcurrentQueue<Processo> processosExecucao = new ConcurrentQueue<Processo>();
        public static ConcurrentQueue<Processo> processosPronto = new ConcurrentQueue<Processo>();

        private static void Main(string[] args)
        {
            var a = new Thread(new ThreadStart(CriaProcessos));
            a.Start();

            var c = new Thread(new ThreadStart(BuscarProcessosEsperando));
            c.Start();

            var b = new Thread(new ThreadStart(BuscarProcessosProntos));
            b.Start();

            var e = new Thread(new ThreadStart(ExecutarProcesso));
            e.Start();

            while (true)
            {
            }
        }

        public static void CriaProcessos()
        {
            while (true)
            {
                Random rd = new Random();
                int rand_num = rd.Next(1, 10);
                Processo abc = new Processo()
                {
                    nome = "Processo " + rand_num,
                    tempo = rand_num
                };

                processosEspera.Enqueue(abc);

                Thread.Sleep(1000);
            }
        }

        public static void BuscarProcessosEsperando()
        {
            while (true)
            {
                if (processosEspera.Count > 0)
                {
                    foreach (Processo prc in processosEspera)
                    {
                        int tempo = prc.tempo;
                        while (tempo > 0)
                        {
                            Console.WriteLine("Na fila de espera: " + prc.nome);
                            tempo -= 1;
                            Thread.Sleep(1000);
                        }

                        processosEspera.TryDequeue(out _);
                        processosPronto.Enqueue(prc);
                    }
                }

                Thread.Sleep(5000);
            }
        }

        public static void BuscarProcessosProntos()
        {
            while (true)
            {
                foreach (Processo prc in processosPronto)
                {
                    if (processosExecucao.Count == 0)
                    {
                        processosExecucao.Enqueue(prc);
                        processosPronto.TryDequeue(out _);
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public static void ExecutarProcesso()
        {
            while (true)
            {
                if (processosExecucao.Count > 0)
                {
                    Processo prc = processosExecucao.FirstOrDefault();
                    int tempo = prc.tempo;
                    while (tempo > 0)
                    {
                        Console.WriteLine("Executando " + prc.nome);
                        tempo -= 1;
                        Thread.Sleep(1000);
                    }

                    processosExecucao.TryDequeue(out _);
                }
            }
        }
    }
}
