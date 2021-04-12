-- phpMyAdmin SQL Dump
-- version 5.1.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 13, 2021 at 12:50 AM
-- Server version: 10.4.18-MariaDB
-- PHP Version: 7.3.27

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ms_turnierservice`
--

-- --------------------------------------------------------

--
-- Table structure for table `turnier`
--

CREATE TABLE `turnier` (
  `id` int(11) NOT NULL,
  `Titel` varchar(33) DEFAULT NULL,
  `Sportart` varchar(33) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `turnier`
--

INSERT INTO `turnier` (`id`, `Titel`, `Sportart`) VALUES
(3, 'Foo', 'Fussball'),
(4, 'Bar', 'Handball');

-- --------------------------------------------------------

--
-- Table structure for table `turnierspiel`
--

CREATE TABLE `turnierspiel` (
  `id` int(11) NOT NULL,
  `TurnierId` int(33) DEFAULT NULL,
  `ErsterTeilnehmerId` int(33) DEFAULT NULL,
  `ErsterTeilnehmerPunkte` int(33) DEFAULT NULL,
  `ZweiterTeilnehmerId` int(33) DEFAULT NULL,
  `ZweiterTeilnehmerPunkte` int(33) DEFAULT NULL,
  `Position` int(33) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `turnierspiel`
--

INSERT INTO `turnierspiel` (`id`, `TurnierId`, `ErsterTeilnehmerId`, `ErsterTeilnehmerPunkte`, `ZweiterTeilnehmerId`, `ZweiterTeilnehmerPunkte`, `Position`) VALUES
(5, 3, 16, 11, 17, 12, 1),
(6, 3, 1, 0, 20, 2, 2),
(7, 4, 18, 4, 2, 15, 1),
(8, 4, 19, 6, 21, 21, 2);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `turnier`
--
ALTER TABLE `turnier`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `turnierspiel`
--
ALTER TABLE `turnierspiel`
  ADD PRIMARY KEY (`id`),
  ADD KEY `TurnierId` (`TurnierId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `turnier`
--
ALTER TABLE `turnier`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `turnierspiel`
--
ALTER TABLE `turnierspiel`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `turnierspiel`
--
ALTER TABLE `turnierspiel`
  ADD CONSTRAINT `turnierspiel_ibfk_1` FOREIGN KEY (`TurnierId`) REFERENCES `turnier` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
