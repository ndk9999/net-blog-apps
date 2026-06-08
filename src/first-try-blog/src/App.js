import { BrowserRouter, Route, Routes } from "react-router-dom";
// import Header from "./components/Header";
import Layout from "./components/Layout";
// import Author from "./components/Author";
// import EmailInput from "./components/EmailInput";
// import Counter from "./components/Counter";
// import NewsletterField from "./components/NewsletterField";
// import LimitedLengthInput from "./components/LimitedLengthInput";
// import TagCloud from "./components/TagCloud";
// import Add from "./components/Add";
// import Subtract from "./components/Subtract";
// import Multiply from "./components/Multiply";
// import Divide from "./components/Divide";
// import Calculator from "./components/Calculator";
// import TermsOfUse from "./components/TermsOfUse";
// import TodoList from "./components/TodoList";
// import LoginForm from "./components/LoginForm";
// import CurrentTime from "./components/CurrentTime";
import Blog from "./pages/Blog";
import Dashboard from "./pages/Dashboard";
import TodoList from "./pages/TodoList";

function App() {
  return (
    <BrowserRouter>
        <Layout>
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/blog/:id" element={<Blog />} />
            <Route path="/blog" element={<Blog />} />
            <Route path="/todolist" element={<TodoList />} />
          </Routes>
        </Layout>
    </BrowserRouter>
  );
}

export default App;
