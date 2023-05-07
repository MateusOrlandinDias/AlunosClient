import React from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Login from './pages/Login';
import Alunos from './pages/Alunos';
import NovoAluno from './pages/NovoAluno';

export default function RoutesApp() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" exact element={<Login />} />
                <Route path="/alunos" exact element={<Alunos />} />
                <Route path="/aluno/novo/:alunoId" exact element={<NovoAluno />} />
            </Routes>
        </BrowserRouter>
    );
}