import React, { useState } from 'react';
import { View, StyleSheet, FlatList } from 'react-native';
import {
  Appbar,
  Searchbar,
  Card,
  Avatar,
  Text,
  Button,
  Provider as PaperProvider,
  DefaultTheme,
} from 'react-native-paper';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';

// Mock data for demonstrating the UI with a list of users.
const users = [
  { id: '1', name: 'John Doe', email: 'john.doe@example.com', online: true },
  { id: '2', name: 'Jane Smith', email: 'jane.smith@example.com', online: false },
  { id: '3', name: 'Peter Jones', email: 'peter.jones@example.com', online: true },
  { id: '4', name: 'Sam Wilson', email: 'sam.wilson@example.com', online: true },
];

// Define the trust-inducing color palette as requested.
const theme = {
  ...DefaultTheme,
  colors: {
    ...DefaultTheme.colors,
    primary: '#0A2342', // Authoritative Dark Blue
    accent: '#D4AF37',   // Authoritative Gold
    background: '#FFFFFF',
    surface: '#F8F9FA', // Light grey for surfaces
    text: '#1C1C1E',     // Deep Grey for text
    placeholder: '#6E6E73',
  },
};

/**
 * A single-screen presentation of the Admin Center UI.
 */
const AdminCenterScreen = () => {
  const [searchQuery, setSearchQuery] = useState('');

  const onChangeSearch = query => setSearchQuery(query);

  const filteredUsers = users.filter(user =>
    user.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    user.email.toLowerCase().includes(searchQuery.toLowerCase())
  );

  /**
   * Renders a single user card with their information and available actions.
   */
  const UserCard = ({ user }) => (
    <Card style={styles.card}>
      <Card.Content>
        <View style={styles.cardHeader}>
          <Avatar.Icon size={48} icon="account-circle" style={styles.avatar} />
          <View style={styles.userInfo}>
            <View style={styles.userNameContainer}>
              <Text style={styles.userName}>User: {user.name}</Text>
              {user.online && <View style={styles.onlineIndicator} />}
            </View>
            <Text style={styles.userEmail}>{user.email}</Text>
          </View>
        </View>
      </Card.Content>
      <Card.Actions style={styles.cardActions}>
        <Button icon={() => <Icon name="account-plus-outline" size={16} color={theme.colors.primary} />} mode="outlined" onPress={() => {}} style={styles.button} labelStyle={styles.buttonLabel}>
          ASSIGN ROLE
        </Button>
        <Button icon={() => <Icon name="account-minus-outline" size={16} color={theme.colors.primary} />} mode="outlined" onPress={() => {}} style={styles.button} labelStyle={styles.buttonLabel}>
          REMOVE ROLE
        </Button>
        <Button icon={() => <Icon name="shield-key-outline" size={16} color={theme.colors.accent} />} mode="outlined" onPress={() => {}} style={[styles.button, {borderColor: theme.colors.accent}]} labelStyle={[styles.buttonLabel, {color: theme.colors.accent}]}>
          SET CLAIM
        </Button>
        <Button icon={() => <Icon name="shield-off-outline" size={16} color={theme.colors.accent} />} mode="outlined" onPress={() => {}} style={[styles.button, {borderColor: theme.colors.accent}]} labelStyle={[styles.buttonLabel, {color: theme.colors.accent}]}>
          REMOVE CLAIM
        </Button>
      </Card.Actions>
    </Card>
  );

  return (
    <PaperProvider theme={theme}>
      <View style={styles.container}>
        <Appbar.Header style={{ backgroundColor: theme.colors.primary }}>
          <Appbar.BackAction onPress={() => {}} color="white" />
          <Appbar.Content title="Admin Center" titleStyle={{ color: 'white' }} />
        </Appbar.Header>
        
        <View style={styles.content}>
          <Searchbar
            placeholder="Search users..."
            onChangeText={onChangeSearch}
            value={searchQuery}
            style={styles.searchbar}
          />
          <FlatList
            data={filteredUsers}
            renderItem={({ item }) => <UserCard user={item} />}
            keyExtractor={item => item.id}
            contentContainerStyle={styles.listContainer}
            showsVerticalScrollIndicator={false}
          />
        </View>
      </View>
    </PaperProvider>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.colors.background,
  },
  content: {
    flex: 1,
    paddingHorizontal: 16,
  },
  searchbar: {
    marginTop: 16,
    marginBottom: 8,
    backgroundColor: theme.colors.surface,
    elevation: 2,
  },
  listContainer: {
    paddingTop: 8,
    paddingBottom: 16,
  },
  card: {
    marginBottom: 16,
    backgroundColor: 'white',
    elevation: 3,
  },
  cardHeader: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  avatar: {
    backgroundColor: theme.colors.primary,
  },
  userInfo: {
    marginLeft: 16,
    flex: 1,
  },
  userNameContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  userName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: theme.colors.text,
  },
  onlineIndicator: {
    width: 9,
    height: 9,
    borderRadius: 5,
    backgroundColor: '#28A745', // Online green
    marginLeft: 8,
    borderWidth: 1,
    borderColor: 'white',
  },
  userEmail: {
    fontSize: 14,
    color: theme.colors.placeholder,
  },
  cardActions: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
    padding: 8,
    backgroundColor: theme.colors.surface,
  },
  button: {
    margin: 4,
    flexBasis: '46%', // Ensure two buttons fit per row with space
    borderColor: theme.colors.primary,
  },
  buttonLabel: {
    color: theme.colors.primary,
    fontSize: 11,
    fontWeight: 'bold',
  },
});

export default AdminCenterScreen;