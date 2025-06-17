import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { getPublicPost } from '../../services/postService';
import './PostDetailPage.css';
import NavBar from '../../components/shared/NavBar';

const PostDetailPage = () => {
  const { id } = useParams();
  const [post, setPost] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPost = async () => {
      try {
        const response = await getPublicPost(id);
        setPost(response.data);
      } catch (err) {
        setError('Failed to fetch post details.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchPost();
  }, [id]);

  if (loading) return <div>Loading post...</div>;
  if (error) return <div className="error-message">{error}</div>;
  if (!post) return <div>Post not found.</div>

  return (
    <>
    <NavBar/>
    <div className="post-detail-page">
      <h1 className="post-detail-title">{post.title}</h1>
      <img src={post.image || 'https://via.placeholder.com/800x400'} alt={post.title} className="post-detail-image" />
      <div className="post-detail-body" dangerouslySetInnerHTML={{ __html: post.body }}></div>
      <div className="post-detail-meta">
        <span>Published on: {new Date(post.publishDate).toLocaleDateString()}</span>
      </div>
    </div>
    </>
  );
};

export default PostDetailPage;