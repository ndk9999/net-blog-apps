import Layout from "./components/shared/Layout";
import Home from "./pages/Home";
import Contact from "./pages/Contact";
import About from "./pages/About";
import Blog from "./pages/Blog";
import SinglePost from "./pages/SinglePost";
import PostByAuthor from "./pages/PostByAuthor";
import PostByCategory from "./pages/PostByCategory";
import PostByTag from "./pages/PostByTag";
import PostByTime from "./pages/PostByTime";
import NotFound from "./components/shared/NotFound";
import { BrowserRouter, Route, Routes } from "react-router-dom";
// import SinglePost from "./pages/SinglePost";
// import NotFound from './components/shared/NotFound';

function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/about-us" element={<About />} />
          <Route path="/contact" element={<Contact />} />
          <Route path="/posts" element={<Blog />} />
          <Route path="/post/:slug" element={<SinglePost />} />
          <Route path="/tag/:slug" element={<PostByTag />} />
          <Route path="/category/:slug" element={<PostByCategory />} />
          <Route path="/author/:slug" element={<PostByAuthor />} />
          <Route path="/archives/:year/:month" element={<PostByTime />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

export default App;
