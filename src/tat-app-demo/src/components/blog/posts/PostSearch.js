import React, { useEffect, useState } from 'react'
import Pager from '../../controls/Pager';
import PostGrid from './PostGrid';

const PostSearch = ({postQuery}) => {
    const {keyword, authorSlug, tagSlug, categorySlug, year, month} = postQuery;

    const [isLoading, setIsLoading] = useState(true);
    const [pageNumber, setPageNumber] = useState(1);
    const [postsList, setPostsList] = useState({
        items: [],
        metadata: {}
    });

    useEffect(() => {
        setIsLoading(true);
        loadBlogPosts();

        async function loadBlogPosts() {
            const parameters = new URLSearchParams({
                keyword: keyword || '', 
                authorSlug: authorSlug || '', 
                tagSlug: tagSlug || '', 
                categorySlug: categorySlug || '', 
                year: year || 0, 
                month: month || 0, 
                pageNumber: pageNumber || 1, 
                pageSize: 10
            });
            const apiEndpoint = `https://localhost:7085/api/posts?${parameters}`;
            const response = await fetch(apiEndpoint);
            const data = await response.json();

            setPostsList(data.result);
            setIsLoading(false);
            window.scrollTo(0, 0)
        }
    }, [keyword, authorSlug, tagSlug, categorySlug, year, month, pageNumber]);

    function updatePageNumber(inc) {
        setPageNumber(prevVal => prevVal + inc);
    }

    return (
        <div className="posts-wrapper">
            {postsList.items.length ? (
                <>
                    <PostGrid articles={postsList.items} />
                    <Pager
                        pageCount={postsList.metadata.pageCount}
                        hasNextPage={postsList.metadata.hasNextPage || false}
                        hasPrevPage={postsList.metadata.hasPreviousPage || false}
                        onPageChange={updatePageNumber} />
                </>
            ) : (
                <p className='text-center text-danger fw-bold'>
                    { isLoading ? "Loading posts" : "No blog posts found" }
                </p>
            )}
        </div>
    )
}

export default PostSearch;