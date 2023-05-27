import React, { useState, useEffect } from 'react';
import './styles.css';
import logoCadastro from '../../assets/cadastro.png';
import { Link, useNavigate } from 'react-router-dom';
import { FiXCircle, FiEdit, FiUserX } from 'react-icons/fi';
import api from '../../services/api';

export default function Alunos() {
    //filtrar dados
    const [searchInput, setSearchInput] = useState('');
    const [filtro, setFiltro] = useState([]);

    const [alunos, setAlunos] = useState([]);

    const email = localStorage.getItem('email');
    const token = localStorage.getItem('token');

    const history = useNavigate();

    const authorization = {
        headers: {
            Authorization: `Bearer ${token}`
        }
    }

    useEffect(()=>{
        if (searchInput !== '') {
            console.log('a');
            console.log(searchInput);
            const dadosFiltrados = alunos.filter((item) => {
                console.log(Object.values(item.nome))
                return Object.values(item).join('').toLowerCase()
                    .includes(searchInput.toLowerCase());
            });
            console.log(dadosFiltrados);
            setFiltro(dadosFiltrados);
        } else {
            console.log('b');
            setFiltro(alunos);
        }
    }, [searchInput])

    useEffect(() => {
        api.get('api/alunos', authorization)
            .then(response => {
                setAlunos(response.data);
            }, token)
    })

    async function logout() {
        try {
            localStorage.clear();
            localStorage.setItem('token', '');
            authorization.headers = '';
            history('/');
        } catch (error) {
            alert('Não foi possivel fazer o logout: ' + error);
        }
    }

    async function editAluno(id) {
        try {
            history(`/aluno/novo/${id}`);
        } catch (error) {
            alert('Não foi possível editar o aluno: ' + error);
        }
    }

    return (
        <div className='aluno-container'>
            <header>
                <img src={logoCadastro} alt="Cadastro" />
                <span>Bem-Vindo(a) <strong>{email}</strong>!</span>
                <Link className='button' to="/aluno/novo/0">Novo Aluno</Link>
                <button onClick={logout} type="button">
                    <FiXCircle size={35} color="#17202a" />
                </button>
            </header>
            <form>
                <input type="text" placeholder='Filtrar por nome...'
                    onChange={e=>setSearchInput(e.target.value)}
                />
            </form>
            <h1>Relação de Alunos</h1>
            {searchInput.length > 1 ? (
                <ul>
                    {filtro.map(aluno => (
                        <li key={aluno.id}>
                            <b>Nome: </b>{aluno.nome}<br /><br />
                            <b>Email: </b>{aluno.email}<br /><br />
                            <b>Idade: </b>{aluno.idade}<br /><br />
                            <button onClick={() => editAluno(aluno.id)} type='button'>
                                <FiEdit size="25" color="17202a" />
                            </button>
                            <button type='button'>
                                <FiUserX size="25" color="17202a" />
                            </button>
                        </li>
                    ))}
                </ul>
            ) : (
                <ul>
                    {alunos.map(aluno => (
                        <li key={aluno.id}>
                            <b>Nome: </b>{aluno.nome}<br /><br />
                            <b>Email: </b>{aluno.email}<br /><br />
                            <b>Idade: </b>{aluno.idade}<br /><br />
                            <button onClick={() => editAluno(aluno.id)} type='button'>
                                <FiEdit size="25" color="17202a" />
                            </button>
                            <button type='button'>
                                <FiUserX size="25" color="17202a" />
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}