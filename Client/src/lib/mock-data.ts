import type { Lecture, Course } from "./types"

const mlLectures: Lecture[] = [
  {
    id: "1",
    name: "Introduction to Machine Learning",
    date: "2025-01-15",
    duration: "1:45:30",
    transcript: `Welcome to Introduction to Machine Learning. Today we'll cover the fundamentals of supervised learning, unsupervised learning, and reinforcement learning.

Machine learning is a subset of artificial intelligence that enables systems to learn and improve from experience without being explicitly programmed. The key concept here is that machines can identify patterns in data.

Let's start with supervised learning. In supervised learning, we train our model on labeled data. For example, if we want to classify emails as spam or not spam, we provide the algorithm with examples of both spam and legitimate emails, each labeled accordingly.

Neural networks are a fundamental architecture in deep learning. They consist of layers of interconnected nodes, or neurons, that process information. The input layer receives data, hidden layers process it, and the output layer produces the final result.

Gradient descent is the optimization algorithm we use to minimize the loss function. It iteratively adjusts the model's parameters to find the optimal values that minimize prediction errors.

Overfitting occurs when a model learns the training data too well, including its noise and outliers. This results in poor performance on new, unseen data. We use techniques like regularization and cross-validation to prevent overfitting.`,
    summary:
      "This lecture introduces the core concepts of machine learning, including supervised learning, neural networks, and optimization techniques. Key topics covered include gradient descent, overfitting prevention, and the fundamental architecture of neural networks.",
    keywords: [
      {
        term: "Machine Learning",
        occurrences: [
          { timestamp: "00:02:15", context: "Machine learning is a subset of artificial intelligence..." },
          { timestamp: "00:15:30", context: "The key concept in machine learning is pattern recognition..." },
        ],
      },
      {
        term: "Supervised Learning",
        occurrences: [
          { timestamp: "00:05:45", context: "In supervised learning, we train our model on labeled data..." },
          { timestamp: "00:08:20", context: "Supervised learning requires examples with known outcomes..." },
        ],
      },
      {
        term: "Neural Networks",
        occurrences: [
          { timestamp: "00:12:10", context: "Neural networks are a fundamental architecture in deep learning..." },
          { timestamp: "00:18:45", context: "The neural network consists of layers of interconnected nodes..." },
        ],
      },
      {
        term: "Gradient Descent",
        occurrences: [
          { timestamp: "00:25:30", context: "Gradient descent is the optimization algorithm we use..." },
          { timestamp: "00:28:15", context: "Through gradient descent, we minimize the loss function..." },
        ],
      },
      {
        term: "Overfitting",
        occurrences: [
          { timestamp: "00:35:20", context: "Overfitting occurs when a model learns the training data too well..." },
          { timestamp: "00:37:45", context: "To prevent overfitting, we use regularization techniques..." },
        ],
      },
    ],
    flashcards: [
      {
        id: "1-1",
        question: "What is Machine Learning?",
        answer:
          "Machine learning is a subset of artificial intelligence that enables systems to learn and improve from experience without being explicitly programmed. The key concept is that machines can identify patterns in data.",
        category: "Fundamentals",
        relatedKeyword: "Machine Learning",
      },
      {
        id: "1-2",
        question: "What is Supervised Learning?",
        answer:
          "Supervised learning is a type of machine learning where we train our model on labeled data. For example, to classify emails as spam or not spam, we provide the algorithm with examples of both types, each labeled accordingly.",
        category: "Learning Types",
        relatedKeyword: "Supervised Learning",
      },
      {
        id: "1-3",
        question: "What are Neural Networks?",
        answer:
          "Neural networks are a fundamental architecture in deep learning. They consist of layers of interconnected nodes (neurons) that process information. The input layer receives data, hidden layers process it, and the output layer produces the final result.",
        category: "Architectures",
        relatedKeyword: "Neural Networks",
      },
      {
        id: "1-4",
        question: "What is Gradient Descent?",
        answer:
          "Gradient descent is the optimization algorithm used to minimize the loss function. It iteratively adjusts the model's parameters to find the optimal values that minimize prediction errors.",
        category: "Optimization",
        relatedKeyword: "Gradient Descent",
      },
      {
        id: "1-5",
        question: "What is Overfitting and how do we prevent it?",
        answer:
          "Overfitting occurs when a model learns the training data too well, including its noise and outliers, resulting in poor performance on new, unseen data. We use techniques like regularization and cross-validation to prevent overfitting.",
        category: "Model Evaluation",
        relatedKeyword: "Overfitting",
      },
    ],
    mediaType: "video",
    mediaUrl: "/placeholder.mp4",
    thumbnailUrl: "/machine-learning-lecture.jpg",
  },
]

const dsaLectures: Lecture[] = [
  {
    id: "2",
    name: "Data Structures and Algorithms",
    date: "2025-01-18",
    duration: "2:10:15",
    transcript: `Welcome to Data Structures and Algorithms. Today's focus is on fundamental data structures and their applications.

Arrays are the most basic data structure. They store elements in contiguous memory locations, allowing for constant-time access by index. However, insertion and deletion operations can be expensive.

Linked lists provide dynamic memory allocation. Unlike arrays, linked lists don't require contiguous memory. Each node contains data and a reference to the next node. This makes insertion and deletion more efficient.

Hash tables offer average constant-time complexity for search, insert, and delete operations. They use a hash function to map keys to array indices. Collision resolution is crucial for maintaining performance.

Binary search trees maintain sorted data and support efficient search operations. The tree property ensures that for any node, all values in the left subtree are smaller, and all values in the right subtree are larger.

Time complexity analysis helps us understand algorithm efficiency. We use Big O notation to describe the worst-case performance. Common complexities include O(1), O(log n), O(n), and O(n²).`,
    summary:
      "This lecture covers fundamental data structures including arrays, linked lists, hash tables, and binary search trees. The session emphasizes time complexity analysis and the trade-offs between different data structure implementations.",
    keywords: [
      {
        term: "Arrays",
        occurrences: [
          { timestamp: "00:03:20", context: "Arrays are the most basic data structure..." },
          { timestamp: "00:06:15", context: "Arrays store elements in contiguous memory locations..." },
        ],
      },
      {
        term: "Linked Lists",
        occurrences: [
          { timestamp: "00:12:40", context: "Linked lists provide dynamic memory allocation..." },
          { timestamp: "00:15:55", context: "Each node in a linked list contains data and a reference..." },
        ],
      },
      {
        term: "Hash Tables",
        occurrences: [
          { timestamp: "00:22:30", context: "Hash tables offer average constant-time complexity..." },
          { timestamp: "00:25:10", context: "The hash function maps keys to array indices..." },
        ],
      },
      {
        term: "Binary Search Trees",
        occurrences: [
          { timestamp: "00:35:45", context: "Binary search trees maintain sorted data..." },
          { timestamp: "00:38:20", context: "The tree property ensures efficient search operations..." },
        ],
      },
      {
        term: "Time Complexity",
        occurrences: [
          { timestamp: "00:48:15", context: "Time complexity analysis helps us understand algorithm efficiency..." },
          { timestamp: "00:52:30", context: "We use Big O notation to describe worst-case performance..." },
        ],
      },
    ],
    flashcards: [
      {
        id: "2-1",
        question: "What are Arrays and what are their characteristics?",
        answer:
          "Arrays are the most basic data structure. They store elements in contiguous memory locations, allowing for constant-time access by index. However, insertion and deletion operations can be expensive.",
        category: "Data Structures",
        relatedKeyword: "Arrays",
      },
      {
        id: "2-2",
        question: "What are Linked Lists and how do they differ from Arrays?",
        answer:
          "Linked lists provide dynamic memory allocation. Unlike arrays, they don't require contiguous memory. Each node contains data and a reference to the next node, making insertion and deletion more efficient than arrays.",
        category: "Data Structures",
        relatedKeyword: "Linked Lists",
      },
      {
        id: "2-3",
        question: "What are Hash Tables and what is their performance?",
        answer:
          "Hash tables offer average constant-time complexity for search, insert, and delete operations. They use a hash function to map keys to array indices. Collision resolution is crucial for maintaining performance.",
        category: "Data Structures",
        relatedKeyword: "Hash Tables",
      },
      {
        id: "2-4",
        question: "What are Binary Search Trees?",
        answer:
          "Binary search trees maintain sorted data and support efficient search operations. The tree property ensures that for any node, all values in the left subtree are smaller, and all values in the right subtree are larger.",
        category: "Data Structures",
        relatedKeyword: "Binary Search Trees",
      },
      {
        id: "2-5",
        question: "What is Time Complexity and Big O notation?",
        answer:
          "Time complexity analysis helps us understand algorithm efficiency. We use Big O notation to describe the worst-case performance. Common complexities include O(1) for constant time, O(log n) for logarithmic, O(n) for linear, and O(n²) for quadratic.",
        category: "Algorithm Analysis",
        relatedKeyword: "Time Complexity",
      },
    ],
    mediaType: "video",
    mediaUrl: "/placeholder.mp4",
    thumbnailUrl: "/data-structures-algorithms.png",
  },
]

const dbLectures: Lecture[] = [
  {
    id: "3",
    name: "Database Design Principles",
    date: "2025-01-22",
    duration: "1:55:45",
    transcript: `Welcome to Database Design Principles. Today we'll explore relational database design and normalization.

Relational databases organize data into tables with rows and columns. Each table represents an entity, and relationships between entities are established through foreign keys.

Normalization is the process of organizing data to reduce redundancy. First normal form requires atomic values in each cell. Second normal form eliminates partial dependencies. Third normal form removes transitive dependencies.

Primary keys uniquely identify each record in a table. They must be unique and not null. Foreign keys establish relationships between tables and maintain referential integrity.

Indexing improves query performance by creating data structures that allow faster data retrieval. However, indexes consume storage space and can slow down write operations.

ACID properties ensure database reliability. Atomicity guarantees all-or-nothing transactions. Consistency maintains data integrity. Isolation prevents concurrent transaction interference. Durability ensures committed transactions persist.`,
    summary:
      "This lecture covers relational database design principles, including normalization, primary and foreign keys, indexing strategies, and ACID properties. The session emphasizes best practices for designing efficient and maintainable database schemas.",
    keywords: [
      {
        term: "Relational Databases",
        occurrences: [
          { timestamp: "00:02:45", context: "Relational databases organize data into tables..." },
          { timestamp: "00:08:30", context: "Each table in a relational database represents an entity..." },
        ],
      },
      {
        term: "Normalization",
        occurrences: [
          { timestamp: "00:15:20", context: "Normalization is the process of organizing data to reduce redundancy..." },
          { timestamp: "00:18:40", context: "Third normal form removes transitive dependencies..." },
        ],
      },
      {
        term: "Primary Keys",
        occurrences: [
          { timestamp: "00:25:15", context: "Primary keys uniquely identify each record in a table..." },
          { timestamp: "00:27:50", context: "Primary keys must be unique and not null..." },
        ],
      },
      {
        term: "Indexing",
        occurrences: [
          { timestamp: "00:38:25", context: "Indexing improves query performance by creating data structures..." },
          { timestamp: "00:42:10", context: "Indexes can slow down write operations..." },
        ],
      },
      {
        term: "ACID Properties",
        occurrences: [
          { timestamp: "00:52:30", context: "ACID properties ensure database reliability..." },
          { timestamp: "00:55:45", context: "Atomicity guarantees all-or-nothing transactions..." },
        ],
      },
    ],
    flashcards: [
      {
        id: "3-1",
        question: "What are Relational Databases?",
        answer:
          "Relational databases organize data into tables with rows and columns. Each table represents an entity, and relationships between entities are established through foreign keys.",
        category: "Database Fundamentals",
        relatedKeyword: "Relational Databases",
      },
      {
        id: "3-2",
        question: "What is Normalization and what are the normal forms?",
        answer:
          "Normalization is the process of organizing data to reduce redundancy. First normal form requires atomic values in each cell. Second normal form eliminates partial dependencies. Third normal form removes transitive dependencies.",
        category: "Database Design",
        relatedKeyword: "Normalization",
      },
      {
        id: "3-3",
        question: "What are Primary Keys and Foreign Keys?",
        answer:
          "Primary keys uniquely identify each record in a table and must be unique and not null. Foreign keys establish relationships between tables and maintain referential integrity.",
        category: "Database Design",
        relatedKeyword: "Primary Keys",
      },
      {
        id: "3-4",
        question: "What is Indexing and what are its trade-offs?",
        answer:
          "Indexing improves query performance by creating data structures that allow faster data retrieval. However, indexes consume storage space and can slow down write operations.",
        category: "Performance",
        relatedKeyword: "Indexing",
      },
      {
        id: "3-5",
        question: "What are ACID Properties?",
        answer:
          "ACID properties ensure database reliability: Atomicity guarantees all-or-nothing transactions, Consistency maintains data integrity, Isolation prevents concurrent transaction interference, and Durability ensures committed transactions persist.",
        category: "Database Fundamentals",
        relatedKeyword: "ACID Properties",
      },
    ],
    mediaType: "audio",
    mediaUrl: "/placeholder.mp3",
    thumbnailUrl: "/database-design-concept.jpg",
  },
]

export const mockCourses: Course[] = [
  {
    id: "cs-401",
    name: "Machine Learning",
    code: "CS 401",
    semester: "Spring 2025",
    instructor: "Dr. Sarah Chen",
    description:
      "Comprehensive introduction to machine learning algorithms, neural networks, and deep learning techniques. Covers supervised and unsupervised learning, optimization, and practical applications.",
    color: "bg-blue-500",
    lectures: mlLectures,
    totalDuration: "1:45:30",
    lectureCount: 1,
  },
  {
    id: "cs-302",
    name: "Data Structures & Algorithms",
    code: "CS 302",
    semester: "Spring 2025",
    instructor: "Prof. Michael Rodriguez",
    description:
      "In-depth study of fundamental data structures and algorithms. Topics include arrays, linked lists, trees, graphs, sorting, searching, and complexity analysis.",
    color: "bg-purple-500",
    lectures: dsaLectures,
    totalDuration: "2:10:15",
    lectureCount: 1,
  },
  {
    id: "cs-350",
    name: "Database Systems",
    code: "CS 350",
    semester: "Spring 2025",
    instructor: "Dr. Emily Watson",
    description:
      "Comprehensive coverage of database design, implementation, and management. Includes relational model, SQL, normalization, indexing, and transaction management.",
    color: "bg-green-500",
    lectures: dbLectures,
    totalDuration: "1:55:45",
    lectureCount: 1,
  },
]

export const mockLectures: Lecture[] = mockCourses.flatMap((course) => course.lectures)
