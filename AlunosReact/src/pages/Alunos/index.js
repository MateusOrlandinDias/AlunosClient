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
    const [updateData, setUpdateData] = useState(true);

    const email = localStorage.getItem('email');
    const token = localStorage.getItem('token');

    const history = useNavigate();

    const authorization = {
        headers: {
            Authorization: `Bearer ${token}`
        }
    }

    useEffect(() => {
        if (searchInput !== '') {
            const dadosFiltrados = alunos.filter((item) => {
                return Object.values(item.nome).join('').toLowerCase()
                    .includes(searchInput.toLowerCase());
            });
            setFiltro(dadosFiltrados);
        } else {
            setFiltro(alunos);
        }
    }, [searchInput])

    useEffect(() => {
        api.get('api/alunos', authorization)
            .then(response => {
                setAlunos(response.data);
            }, token)

        setUpdateData(false);
    }, [updateData])

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

    async function deleteAluno(id) {
        try {
            if (window.confirm('Deseja deletar o aluno de id = ' + id + ' ?')) {
                await api.delete(`api/alunos/${id}`, authorization);
                searchInput !== '' ? setFiltro(filtro.filter(aluno => aluno.id !== id)) : setAlunos(alunos.filter(aluno => aluno.id !== id));
                setUpdateData(true);
            }
        } catch (error) {
            alert('Não foi possível excluir o aluno: ' + error);
            setUpdateData(true);
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
                    onChange={e => setSearchInput(e.target.value)}
                />
            </form>
            <h1>Relação de Alunos</h1>
            {searchInput.length >= 1 ? (
                <ul>
                    {filtro.map(aluno => (
                        <li key={aluno.id}>
                            <b>Nome: </b>{aluno.nome}<br /><br />
                            <b>Email: </b>{aluno.email}<br /><br />
                            <b>Idade: </b>{aluno.idade}<br /><br />
                            <button onClick={() => editAluno(aluno.id)} type='button'>
                                <FiEdit size="25" color="17202a" />
                            </button>
                            <button onClick={() => deleteAluno(aluno.id)} type='button'>
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
                            <button onClick={() => deleteAluno(aluno.id)} type='button'>
                                <FiUserX size="25" color="17202a" />
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}