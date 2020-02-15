using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace VirtualRepositorio
{

    internal class VirtualRepositorio<T> where T : GenericVirtualEntity, new()
    {
        private static VirtualRepositorio<T> instancia = null;
        private Dictionary<string, object> listRepositorios = null;
        private const int tempoParaArmazenamentoFisico = 60; //Em segundos.
        private const bool hasArmazenamentoFisico = true; //Gravar repositorios em Disco.
        private bool isGravando = false;
        private IConfiguration config;

        internal VirtualRepositorio(IConfiguration iConfig)
        {
            this.config = iConfig;
            this.listRepositorios = new Dictionary<string, object>();
            if (hasArmazenamentoFisico)
            {
                Thread threadRepositorio = new Thread(ThreadGravacaoRepositorioFisicamente);
                threadRepositorio.Start();
            }
        }

        /// <summary>
        /// Carregar o repositório do disco físico para memória, a partir do nome do repositório, se existir.
        /// </summary>
        /// <param name="nomeRepositorio">Nome do repositório a ser recuperado.</param>
        private void CarregarRepositorio(string nomeRepositorio)
        {
            
            DirectoryInfo dirRepositorio = new DirectoryInfo(config.GetSection("ConfigApp").GetSection("LocalRepositorioVirtual").Value);
            if (dirRepositorio.Exists)
            {//Verificar existencia de repositorios...
                var fileRepositorio = new FileInfo(string.Format("{0}\\{1}.rep", dirRepositorio.FullName, nomeRepositorio));
                if (fileRepositorio.Exists)
                {
                    try
                    {
                        RepositorioSerializer<T> resultRepositorio;
                        IFormatter formatter = new BinaryFormatter();
                        Stream stream = new FileStream(fileRepositorio.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                        resultRepositorio = (RepositorioSerializer<T>)formatter.Deserialize(stream);
                        stream.Close();
                        Repositorio<T> repositorio = new Repositorio<T>();
                        repositorio.Carregar(resultRepositorio.Registros);
                        this.listRepositorios.Add(nomeRepositorio, repositorio);
                    }
                    catch (Exception erro)
                    {
                        Console.Error.WriteLine(erro.Message);
                        //Erro ao Deserialize;
                    }
                }
            }
        }

        /// <summary>
        /// Retorna a instância do repositório virtual.
        /// </summary>
        /// <returns></returns>
        public static VirtualRepositorio<T> Get(IConfiguration iConfig)
        {
            if (instancia == null)
            {
                instancia = new VirtualRepositorio<T>(iConfig);
            }
            return instancia;
        }

        /// <summary>
        /// Thred que ficará gravando os repositórios fisicamente em disco.
        /// </summary>
        private void ThreadGravacaoRepositorioFisicamente()
        {
            while (true)
            {
                try
                {

                    if (isGravando == false)
                    {
                        isGravando = true;
                        foreach (var nomeRepositorio in listRepositorios.Keys)
                        {
                            Repositorio<T> repositorio = listRepositorios[nomeRepositorio] as Repositorio<T>;
                            if (repositorio.HasGravar())
                            {
                                GravarRepositorio(nomeRepositorio, repositorio);
                                repositorio.SetGravado();
                            }
                        }
                        isGravando = false;
                    }
                    Thread.Sleep(6000 * tempoParaArmazenamentoFisico); //6000 = um segundo.
                }
                catch (Exception erro)
                {
                    Console.Error.WriteLine(erro.Message);
                    //Thread de gravação dos repositórios!
                }
            }
        }

        /// <summary>
        /// Realiza a gravação do repositório virtual fisicamente em disco.
        /// </summary>
        /// <param name="nomeRepositorio">Nome do repositório.</param>
        /// <param name="repositorio">Repositório a ser armazenado.</param>
        private void GravarRepositorio(string nomeRepositorio, Repositorio<T> repositorio)
        {
            try
            {
                DirectoryInfo dirRepositorio = new DirectoryInfo(config.GetSection("ConfigApp").GetSection("LocalRepositorioVirtual").Value);
                if (!dirRepositorio.Exists) dirRepositorio.Create();
                FileInfo pathRepositorio = new FileInfo(string.Format("{0}\\{1}.rep", dirRepositorio.FullName, nomeRepositorio));
                int qtdRegistros = repositorio.QtdRegistros();
                if (qtdRegistros == 0)
                {//Nenhum registro para registrar dados físicos.
                    if (pathRepositorio.Exists)
                    {//Existe arquivo do repositório... remover.
                        pathRepositorio.Delete();
                    }
                    return;
                }

                //Armazenar repositório físico...
                RepositorioSerializer<T> repositorioSerializer = new RepositorioSerializer<T>();
                repositorioSerializer.QtdRegistros = qtdRegistros;
                repositorioSerializer.Registros = new Registros<T>();
                foreach (var item in repositorio.Listar())
                    repositorioSerializer.Registros.Add(item);
                repositorioSerializer.DataHoraUltimaAtualizacao = DateTime.Now;
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(pathRepositorio.FullName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, repositorioSerializer);
                stream.Close();
            }
            catch (Exception erro)
            {
                Console.Error.WriteLine(erro.Message);
                //Repositório não armazenado!
            }
        }

        /// <summary>
        /// Recupera o repositório de Virtual de um tipo específico.
        /// </summary>
        /// <returns></returns>
        public Repositorio<T> GetRepositorio()
        {

            string nomeRepositorio = typeof(T).FullName;
            bool hasExisteRepositorio = listRepositorios.ContainsKey(nomeRepositorio);
            if (!hasExisteRepositorio && hasArmazenamentoFisico)
            {//Verificar existência de repositório armazenado fisicamente.
                CarregarRepositorio(nomeRepositorio);
            }

            //Verificar existência de repositório na memória.
            hasExisteRepositorio = listRepositorios.ContainsKey(nomeRepositorio);
            if (hasExisteRepositorio)
            {//Repositorio existente...
                return listRepositorios[nomeRepositorio] as Repositorio<T>;
            }
            else
            {//Criar novo repositorio...
                var _newRepositorio = new Repositorio<T>();
                listRepositorios.Add(nomeRepositorio, _newRepositorio);
                return _newRepositorio;
            }

        }

    }
    [Serializable]
    public class GenericVirtualEntity
    {
        public int Id { get; set; }
    }
    internal interface IRepositorio<T> where T : GenericVirtualEntity
    {
        T Incluir(T entity);
        void Excluir(T entity);
        void Alterar(T entity);
        T Obter(Func<T, bool> where);
        List<T> Listar();
        List<T> Listar(Func<T, bool> where);
    }

    internal class Repositorio<T> : IRepositorio<T> where T : GenericVirtualEntity
    {
        private List<T> listEntites = null;
        private int ultimoId = 0;
        private bool isGravar = false;
        internal Repositorio()
        {
            listEntites = new List<T>();
        }

        public T Incluir(T entity)
        {
            if (entity.Id == 0)
                entity.Id = NewId();
            listEntites.Add(entity);
            isGravar = true;
            return entity;
        }

        private int NewId()
        {
            if (ultimoId == 0)
            {
                Random random = new Random(DateTime.Now.Millisecond);
                return ultimoId = random.Next(101, 999999);
            }
            return ultimoId += 1;
        }

        public void Excluir(T entity)
        {
            listEntites.Remove(entity);
            isGravar = true;
        }

        public void Alterar(T entity)
        {
            var _entiry = Obter(t => t.Id == entity.Id);
            if (_entiry != null)
            {
                var idx = listEntites.IndexOf(_entiry);
                if (idx != -1)
                {//Encontrado... substituir objeto.
                    listEntites[idx] = entity;
                }
                isGravar = true;
            }
        }

        public T Obter(Func<T, bool> where)
        {
            return listEntites.Where(where).FirstOrDefault();
        }

        public List<T> Listar()
        {
            return Listar(null);
        }
        public List<T> Listar(Func<T, bool> where)
        {
            if (where == null)
            {
                return listEntites.ToList();
            }
            return listEntites.Where(where).ToList();
        }

        public int QtdRegistros()
        {
            return listEntites.Count();
        }

        public int QtdRegistros(Func<T, bool> where)
        {
            return listEntites.Count(where);
        }

        internal void Carregar(Registros<T> registros)
        {
            for (int i = 0; i < registros.Count; i++)
            {
                this.listEntites.Add(registros[i]);
                ultimoId = registros[i].Id;
            }
        }
        internal bool HasGravar()
        {
            return isGravar;
        }
        internal void SetGravado()
        {
            isGravar = false;
        }
    }

    [Serializable]
    internal class RepositorioSerializer<T> where T : new()
    {
        public int QtdRegistros { get; set; }
        public Registros<T> Registros { get; set; }
        public DateTime DataHoraUltimaAtualizacao { get; set; }
    }

    [Serializable]
    internal class Registros<T> : ICollection
    {
        public string CollectionName;
        private ArrayList array = new ArrayList();

        public T this[int index]
        {
            get { return (T)array[index]; }
        }

        public void CopyTo(Array a, int index)
        {
            array.CopyTo(a, index);
        }
        public int Count
        {
            get { return array.Count; }
        }
        public object SyncRoot
        {
            get { return this; }
        }
        public bool IsSynchronized
        {
            get { return false; }
        }
        public IEnumerator GetEnumerator()
        {
            return array.GetEnumerator();
        }
        public void Add(T registro)
        {
            array.Add(registro);
        }
    }

}