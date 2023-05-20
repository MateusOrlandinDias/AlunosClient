import React, { useState } from 'react';
import './styles.css';
import logoImage from '../../assets/senha.png';
import api from '../../services/api';
import { useNavigate } from 'react-router-dom';

export default function Login() {

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const history = useNavigate();

    /*Login recebe um evento e usa o prevent default para que quando clique no botao de login a pagina não dê refresh
    O email e a password serão enviados em formato de json para a API
    */
    async function login(event) {
        event.preventDefault();

        const data = {
            email, password
        }

        try {
            const response = await api.post('api/account/loginuser', data);
            /*Se não retornar erro de autenticação, a api vai gerar o jwt e podemos armazenar no local storage para usar nos headers 
            das proximas requisições*/
            localStorage.setItem('email', email);
            localStorage.setItem('token', response.data.token);
            localStorage.setItem('expiration', response.data.expiration);
            /*Quando o login for feito com sucesso, ele manda pra rota /alunos*/
            history('/alunos');

        } catch (error) {
            alert('O login falhou: ' + error);
        }
    }

    return (
        <div className="login-container">
            <section className="form">
                <img src={logoImage} alt="Login" id="img1" />
                <form onSubmit={login}>
                    <h1>Cadastro de Alunos</h1>
                    <input placeholder="Email"
                        value={email}
                        onChange={e => setEmail(e.target.value)}
                    />
                    <input type="password" placeholder="Password"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                    />
                    <button className="button" type="submit">Login</button>
                </form>
            </section>
        </div>
    );
}