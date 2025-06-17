import React, { useState, useEffect } from 'react';
import { getPublishedPosts } from '../../services/postService';
import PostCard from '../../components/shared/PostCard';
import './PostListPage.css';
import NavBar from '../../components/shared/NavBar';

const PostListPage = () => {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const response = await getPublishedPosts();
        setPosts(response.data);
      } catch (err) {
        setError('Failed to fetch posts.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchPosts();
  }, []);

  if (loading) return <div>Loading posts...</div>;
  if (error) return <div className="error-message">{error}</div>;

  return (
    <>
    <div className="post-list-page">
      <h1>Our Latest Articles</h1>
      <div className="post-list-container">
        {posts.map(post => (
          <PostCard key={post.id} post={post} />
        ))}
      </div>
    </div>
    </>
  );
};

export default PostListPage;